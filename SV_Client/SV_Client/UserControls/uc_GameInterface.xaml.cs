using SV_Client.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SV_Client.UserControls
{
    /// <summary>
    /// Interaction logic for uc_GameInterface.xaml
    /// </summary>
    public partial class uc_GameInterface : UserControl
    {
        private int pr_AmountOfSize4Ships;
        private int pr_AmountOfSize3Ships;
        private int pr_AmountOfSize2Ships;
        private int pr_AmountOfSize1Ships;
        private int pr_AmountOfShips;
        private bool pr_DirectionChange;
        private bool pr_ModifyShip;
        private bool pr_ModifiedShipDirection;
        private bool pr_RightClickLimiter;
        private bool pr_PreviewDirectionChanged;
        private bool pr_IsItMyTurn;
        private bool pr_AttackHitMiss;
        private int pr_CurrentPreviewField;
        private Rectangle pr_CurrentPreviewShip;
       

        private Point pr_StartPoint;
        private UIElement pr_OriginalElement;
        private Grid pr_SourceGrid;
        private Canvas pr_OwnCanvas;
        private double pr_ScaleWidth;
        private double pr_ScaleHeight;
        private List<Field> pr_OwnGameFields;
        private List<Field> pr_OpponentGameFields;
        private List<TestShip> pr_ShipList;

        private TcpClient pr_TCPServerStation;
        private int pr_TCPServerPort;
        private IPEndPoint pr_TCPServerEndPoint;
        private NetworkStream pr_TCPStream;

        public uc_GameInterface()
        {
            pr_AmountOfSize4Ships = 1;
            pr_AmountOfSize3Ships = 2;
            pr_AmountOfSize2Ships = 2;
            pr_AmountOfSize1Ships = 1;
            pr_AmountOfShips = pr_AmountOfSize4Ships + pr_AmountOfSize3Ships + pr_AmountOfSize2Ships + pr_AmountOfSize1Ships;
            pr_IsItMyTurn = false;
            pr_AttackHitMiss = false;

            pr_DirectionChange = false;
            pr_ModifyShip = false;
            pr_ModifiedShipDirection = false;
            pr_RightClickLimiter = false;
            pr_PreviewDirectionChanged = false;
            pr_CurrentPreviewField = -1;

            pr_OwnGameFields = new List<Field>(240);
            pr_OpponentGameFields = new List<Field>(240);
            pr_ShipList = new List<TestShip>();
            F_InitializeFields();

            pr_TCPServerPort = 50000;

            pr_TCPServerEndPoint = new IPEndPoint(SV_Client.Classes.Client.GeneralInfo.pu_ServerIP, pr_TCPServerPort);

            pr_TCPServerStation = new TcpClient();
            pr_TCPServerStation.Connect(pr_TCPServerEndPoint);

            pr_TCPStream = pr_TCPServerStation.GetStream();

            BackgroundWorker ReceiveDataWorker = new BackgroundWorker();
            ReceiveDataWorker.DoWork += F_ReceiveDataFromServer;
            ReceiveDataWorker.RunWorkerAsync();

            InitializeComponent();
        }

        private void F_ReceiveDataFromServer(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if( pr_TCPStream.CanRead )
                {
                    byte[] buffer = new byte[pr_TCPServerStation.ReceiveBufferSize];

                    pr_TCPStream.Read(buffer, 0, (int)pr_TCPServerStation.ReceiveBufferSize);

                    string receivedData = Encoding.ASCII.GetString(buffer);
                    F_InterpretDataFromServer(receivedData);
                }
            }
        }

        //FUNCTIONS FOR COMMUNICATION WITH SERVER

        private void F_InterpretDataFromServer(string DataToInterpret)
        {
            var DataToInterpretSplitted = DataToInterpret.Split(new[] { "\n\n" }, StringSplitOptions.None);
            string DataToInterpretHeader = DataToInterpretSplitted[0];

            if (DataToInterpretHeader.Split('\n')[0].IndexOf("START [PLAYERNAME]") > 0)
            {
                // ibrprüfung ob spiel startet mit spieler der erstn zug mocht
                string StartingPlayer = ""; // angegebenes feld
                if( StartingPlayer == SV_Client.Classes.Client.GeneralInfo.pu_Username )
                {
                    pr_IsItMyTurn = true;
                }
                else
                {
                    pr_IsItMyTurn = false;
                }
            }
            else if (DataToInterpretHeader.Split('\n')[0].IndexOf("ATTACK MISS") > 0)
            {
                pr_AttackHitMiss = false;
            }
            else if (DataToInterpretHeader.Split('\n')[0].IndexOf("ATTACK HIT") > 0)
            {
                pr_AttackHitMiss = true;
            }
            else if (DataToInterpretHeader.Split('\n')[0].IndexOf("ATTACK ON [FIELD]") > 0)
            {
                int FieldIndex = 0; // angegebenes feld
                F_PlaceAttackOn(FieldIndex, new Graphic.Hit(), pr_OwnCanvas, pr_OwnGameFields);
            }
            else if (DataToInterpretHeader.Split('\n')[0].IndexOf("END WIN") > 0)
            {
                MessageBox.Show("Sie haben das Spiel gewonnen!");
                ViewModels.vm_MainInterface.pu_ChangeGUICommand.Execute(this);
            }
            else if (DataToInterpretHeader.Split('\n')[0].IndexOf("END LOSE") > 0)
            {
                MessageBox.Show("Sie haben das Spiel verloren!");
                ViewModels.vm_MainInterface.pu_ChangeGUICommand.Execute(this);
            }
        }

        private void F_SendDataToServerAndReceive(string DataToSend)
        {
            var sendCode = Encoding.ASCII.GetBytes(DataToSend);
            pr_TCPStream.Write(sendCode, 0, sendCode.Length);

            byte[] buffer = new byte[pr_TCPServerStation.ReceiveBufferSize];
            pr_TCPStream.Read(buffer, 0, (int)pr_TCPServerStation.ReceiveBufferSize);

            var answer = Encoding.ASCII.GetString(buffer);
            F_InterpretDataFromServer(answer);
        }

        private void F_SendDataToServer(string DataToSend)
        {
            var sendCode = Encoding.ASCII.GetBytes(DataToSend);
            pr_TCPStream.Write(sendCode, 0, sendCode.Length);
        }

        // FUNCTIONS FOR EVERY MOUSEINPUT RELATED ANIMATION AND ACTION

        private void ViewList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pr_StartPoint = e.GetPosition(null);
            pr_OriginalElement = (UIElement)e.Source;
            pr_SourceGrid = (sender as Grid);
        }

        private void ViewList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = pr_StartPoint - mousePos;

            if ( (e.LeftButton == MouseButtonState.Pressed) &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance) )
            {
                DragDrop.AddQueryContinueDragHandler(this, QueryContinueDragHandlerStandard);
                DragDrop.DoDragDrop((DependencyObject)e.OriginalSource, "test", DragDropEffects.Move);
                DragDrop.RemoveQueryContinueDragHandler(this, QueryContinueDragHandlerStandard);
            } 
        }

        private void QueryContinueDragHandlerStandard(object sender, QueryContinueDragEventArgs e)
        {
            e.Handled = true;

            if (e.EscapePressed)
            {
                e.Action = DragAction.Cancel;

                return;
            }
            
            if (e.KeyStates.HasFlag(DragDropKeyStates.LeftMouseButton))
            {
                e.Action = DragAction.Continue;
                pr_RightClickLimiter = false;
            }
            else
            {
                e.Action = DragAction.Drop;
            }

            if (e.KeyStates.HasFlag(DragDropKeyStates.RightMouseButton))
            {
                if( pr_RightClickLimiter == false )
                {
                    pr_RightClickLimiter = true;
                    
                    if (pr_ModifyShip == true)
                    {
                        pr_ModifiedShipDirection = !pr_ModifiedShipDirection;
                        pr_PreviewDirectionChanged = true;
                    }
                    else
                    {
                        pr_DirectionChange = !pr_DirectionChange;
                        pr_PreviewDirectionChanged = true;
                    }
                    System.Console.WriteLine("DirC::" + pr_DirectionChange.ToString() + "|||MSD::" + pr_ModifiedShipDirection.ToString());
                }
                
                e.Action = DragAction.Continue;
            }    
        }

        private void OwnGameField_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        private void OwnGameField_Drop(object sender, DragEventArgs e)
        {
            pr_OwnCanvas = (sender as Canvas);
            Point mDropPoint = e.GetPosition(sender as Canvas);

            Rectangle mRect = F_getCopyOfOriginal((pr_OriginalElement as Rectangle));

            F_PlaceShipOn(F_CheckField(mDropPoint.X, mDropPoint.Y, pr_OwnGameFields), mRect, (sender as Canvas), pr_OwnGameFields);
            pr_DirectionChange = false;
            pr_ModifyShip = false;
            pr_RightClickLimiter = false;
        }

        private void GameField_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source.GetType().ToString().Equals("System.Windows.Controls.Canvas"))
            {
                MessageBox.Show("Sie müssen ein Schiff auswählen!");
            }
            else
            {
                pr_StartPoint = e.GetPosition(null);
                pr_OriginalElement = (UIElement)e.Source;
                (sender as Canvas).Children.Remove((UIElement)e.Source);
                int ShipSize = pr_ShipList[F_getShipIndex((UIElement)e.Source)].pr_ShipSize;

                if (ShipSize == 4)
                {
                    pr_AmountOfSize4Ships++;
                    pr_AmountOfShips++;
                    pr_SourceGrid.Children[0].Visibility = Visibility.Visible;
                }
                else if (ShipSize == 3)
                {
                    pr_AmountOfSize3Ships++;
                    pr_AmountOfShips++;
                    pr_SourceGrid.Children[1].Visibility = Visibility.Visible;
                }
                else if (ShipSize == 2)
                {
                    pr_AmountOfSize2Ships++;
                    pr_AmountOfShips++;
                    pr_SourceGrid.Children[2].Visibility = Visibility.Visible;

                }
                else if (ShipSize == 1)
                {
                    pr_AmountOfSize1Ships++;
                    pr_AmountOfShips++;
                    pr_SourceGrid.Children[3].Visibility = Visibility.Visible;

                }

                pr_ShipList.RemoveAt(F_getShipIndex((UIElement)e.Source));
                pr_ModifiedShipDirection = F_getShipDirection(pr_OriginalElement);
                pr_ModifyShip = true;
            }
        }

        private void GameField_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = pr_StartPoint - mousePos;

            if ((e.LeftButton == MouseButtonState.Pressed) &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                DragDrop.AddQueryContinueDragHandler(this, QueryContinueDragHandlerStandard);
                DragDrop.DoDragDrop((DependencyObject)e.OriginalSource, "test", DragDropEffects.Move);
                DragDrop.RemoveQueryContinueDragHandler(this, QueryContinueDragHandlerStandard);
            }
        }

        private void OwnGameField_DragOver(object sender, DragEventArgs e)
        {
            Point mDropPoint = e.GetPosition(sender as Canvas);

            if ((pr_CurrentPreviewField != F_CheckField(mDropPoint.X, mDropPoint.Y, pr_OwnGameFields)) || (pr_PreviewDirectionChanged == true))
            {
                if (pr_CurrentPreviewShip != null)
                {
                    (sender as Canvas).Children.Remove(pr_CurrentPreviewShip);
                }

                pr_CurrentPreviewField = F_CheckField(mDropPoint.X, mDropPoint.Y, pr_OwnGameFields);

                Rectangle TempPreview = F_getCopyOfOriginal((pr_OriginalElement as Rectangle));

                pr_CurrentPreviewShip = TempPreview;

                TempPreview.Opacity = new double();
                TempPreview.Opacity = 0.25;

                F_PreviewShipOn(F_CheckField(mDropPoint.X, mDropPoint.Y, pr_OwnGameFields), TempPreview, (sender as Canvas), pr_OwnGameFields);
            }

        }

        private void ReadyClick(object sender, RoutedEventArgs e)
        {
            if (pr_AmountOfShips == 0)
            {
                pr_OwnCanvas.IsEnabled = false;
                (sender as Button).IsEnabled = false;

                for( var lauf = 0 ; lauf < pr_ShipList.Count ; lauf++)
                {
                    F_SendDataToServer("PUT SHIP\n\n" + XmlSerializer.Serialize<TestShip>(pr_ShipList[lauf]));
                }
            }
            else
            {
                MessageBox.Show("Noch nicht alle Schiffe gesetzt!");
            }
        }

        private void OpponentGamefieldAttack(object sender, MouseButtonEventArgs e)
        {
            if( pr_IsItMyTurn == true )
            {
                Point mAttackPoint = e.GetPosition(sender as Canvas);

                int AttackCoordinates = F_CheckField(mAttackPoint.X, mAttackPoint.Y, pr_OpponentGameFields);

                F_SendDataToServerAndReceive("PUT ATTACK" + AttackCoordinates);

                if (pr_AttackHitMiss == false)
                {
                    F_PlaceAttackOn(AttackCoordinates, new Graphic.Miss(), (sender as Canvas), pr_OpponentGameFields);
                }
                else
                {
                    F_PlaceAttackOn(AttackCoordinates, new Graphic.Hit(), (sender as Canvas), pr_OpponentGameFields);
                }

                pr_IsItMyTurn = false;
            }
            else
            {
                if( pr_OwnCanvas.IsEnabled == false )
                {
                    MessageBox.Show("Sie sind nicht am Zug!");
                }
                else
                {
                    MessageBox.Show("Das Spiel hat noch nicht angefangen!");
                }
                
            }
        }

        // FUNCTIONS FOR SUPPORT OF ANIMATION, ACTION AND FUNCTION FOR SCALING

        private Rectangle F_getCopyOfOriginal(Rectangle original)
        {
            Rectangle Copy = new Rectangle();

            if( pr_ModifyShip == true )
            {
                if (pr_ModifiedShipDirection == false)
                {
                    Copy.Width = original.Width;
                    Copy.Height = original.Height;
                }
                else
                {
                    Copy.Width = original.Height;
                    Copy.Height = original.Width;
                }

                Copy.Fill = original.Fill;
                Copy.StrokeThickness = 2;

                return Copy;
            }
            else
            {
                if (pr_DirectionChange == false)
                {
                    Copy.Width = original.Width;
                    Copy.Height = original.Height;
                }
                else
                {
                    Copy.Width = original.Height;
                    Copy.Height = original.Width;
                }

                Copy.Fill = original.Fill;
                Copy.StrokeThickness = 2;

                return Copy;
            }
        }

        private void F_ScaleInfoUpdate(object sender, SizeChangedEventArgs e)
        {
            pr_ScaleWidth = ViewModels.vm_GameInterface.pustat_ScaleWidth;
            pr_ScaleHeight = ViewModels.vm_GameInterface.pustat_ScaleHeight;
            F_AdaptFields();
        }

        private void F_InitializeFields()
        {
            double AdditiveOnX = 40;
            double AdditiveOnY = 40;

            for( int lauf = 0 ; lauf < 10 ; lauf++ )
            {
                for (int lauf2 = (24 * lauf); lauf2 < (24 * (lauf + 1) ); lauf2++)
                {
                    pr_OwnGameFields.Add(new Field(AdditiveOnX * (lauf2 - (24 * lauf)), AdditiveOnX * ((lauf2 + 1) - (24 * lauf)), AdditiveOnY * lauf, AdditiveOnY * (lauf + 1)));
                    pr_OpponentGameFields.Add(new Field(AdditiveOnX * (lauf2 - (24 * lauf)), AdditiveOnX * ((lauf2 + 1) - (24 * lauf)), AdditiveOnY * lauf, AdditiveOnY * (lauf + 1)));
                }
            }
        }

        private void F_AdaptFields()
        {
            double AdditiveOnX = 40 * pr_ScaleWidth;
            double AdditiveOnY = 40 * pr_ScaleHeight;

            for (int lauf = 0; lauf < 10; lauf++)
            {
                for (int lauf2 = (24 * lauf); lauf2 < (24 * (lauf + 1)); lauf2++)
                {
                    pr_OwnGameFields.Add(new Field(AdditiveOnX * (lauf2 - (24 * lauf)), AdditiveOnX * ((lauf2 + 1) - (24 * lauf)), AdditiveOnY * lauf, AdditiveOnY * (lauf + 1)));
                    pr_OpponentGameFields.Add(new Field(AdditiveOnX * (lauf2 - (24 * lauf)), AdditiveOnX * ((lauf2 + 1) - (24 * lauf)), AdditiveOnY * lauf, AdditiveOnY * (lauf + 1)));
                }
            }
        }

        private int F_CheckField(double X, double Y, List<Field> GameField)
        {
            for (var lauf = 0; lauf < 10; lauf++)
            {
                for (var lauf2 = (24 * lauf); lauf2 < (24 * (lauf + 1)); lauf2++)
                {
                    if ((GameField.ElementAt(lauf2).pu_LowerXBarrier < X && GameField.ElementAt(lauf2).pu_UpperXBarrier > X) &&
                        (GameField.ElementAt(lauf2).pu_LowerYBarrier < Y && GameField.ElementAt(lauf2).pu_UpperYBarrier > Y))
                    {
                        return lauf2;
                    }
                }
            }

            return -1;
        }

        private void F_PlaceShipOn(int FieldIndex, UIElement Ship, Canvas BattleField, List<Field> GameField)
        {
            if( pr_ModifyShip == true )
            {
                bool ShipDirection = F_getShipDirection(Ship);

                if (GameField.ElementAt(FieldIndex).pu_OpenSpace)
                {
                    double XCoordinate = 0;
                    double YCoordinate = 0;

                    if (ShipDirection == false)
                    {
                        XCoordinate = GameField.ElementAt(FieldIndex).pu_LowerXBarrier + (3);
                        YCoordinate = (GameField.ElementAt(FieldIndex).pu_LowerYBarrier + GameField.ElementAt(FieldIndex).pu_UpperYBarrier) / 2;
                        YCoordinate = YCoordinate - 10;
                    }
                    else
                    {
                        XCoordinate = (GameField.ElementAt(FieldIndex).pu_LowerXBarrier + GameField.ElementAt(FieldIndex).pu_UpperXBarrier) / 2;
                        XCoordinate = XCoordinate - 10;
                        YCoordinate = GameField.ElementAt(FieldIndex).pu_LowerYBarrier + (3);
                    }

                    Canvas.SetLeft(Ship, XCoordinate);
                    Canvas.SetTop(Ship, YCoordinate);

                    BattleField.Children.Add(Ship);

                    var ShipSize = F_getShipSize(Ship);

                    if (ShipSize == 4)
                    {

                        pr_AmountOfSize4Ships--;
                        pr_AmountOfShips--;
                        if (pr_AmountOfSize4Ships == 0)
                        {
                            pr_SourceGrid.Children[0].Visibility = Visibility.Hidden;
                        }
                    }
                    else if (ShipSize == 3)
                    {
                        pr_AmountOfSize3Ships--;
                        pr_AmountOfShips--;
                        if (pr_AmountOfSize3Ships == 0)
                        {
                            pr_SourceGrid.Children[1].Visibility = Visibility.Hidden;
                        }
                    }
                    else if (ShipSize == 2)
                    {
                        pr_AmountOfSize2Ships--;
                        pr_AmountOfShips--;
                        if (pr_AmountOfSize2Ships == 0)
                        {
                            pr_SourceGrid.Children[2].Visibility = Visibility.Hidden;
                        }
                    }
                    else if (ShipSize == 1)
                    {
                        pr_AmountOfSize1Ships--;
                        pr_AmountOfShips--;
                        if (pr_AmountOfSize1Ships == 0)
                        {
                            pr_SourceGrid.Children[3].Visibility = Visibility.Hidden;
                        }
                    }

                    pr_ShipList.Add(new TestShip(FieldIndex, pr_DirectionChange, ShipSize, Ship));
                }
                else
                {
                    //MessageBox.Show("Schiff kann nicht auf dieses Feld gesetzt werden!");
                }
            }
            else
            {
                if (GameField.ElementAt(FieldIndex).pu_OpenSpace)
                {
                    double XCoordinate = 0;
                    double YCoordinate = 0;

                    if (pr_DirectionChange == false)
                    {
                        XCoordinate = GameField.ElementAt(FieldIndex).pu_LowerXBarrier + (3);
                        YCoordinate = (GameField.ElementAt(FieldIndex).pu_LowerYBarrier + GameField.ElementAt(FieldIndex).pu_UpperYBarrier) / 2;
                        YCoordinate = YCoordinate - 10;
                    }
                    else
                    {
                        XCoordinate = (GameField.ElementAt(FieldIndex).pu_LowerXBarrier + GameField.ElementAt(FieldIndex).pu_UpperXBarrier) / 2;
                        XCoordinate = XCoordinate - 10;
                        YCoordinate = GameField.ElementAt(FieldIndex).pu_LowerYBarrier + (3);
                    }

                    Canvas.SetLeft(Ship, XCoordinate);
                    Canvas.SetTop(Ship, YCoordinate);

                    BattleField.Children.Add(Ship);

                    var ShipSize = F_getShipSize(Ship);

                    if( ShipSize == 4 )
                    {
                        
                        pr_AmountOfSize4Ships--;
                        pr_AmountOfShips--;
                        if( pr_AmountOfSize4Ships == 0 )
                        {
                            pr_SourceGrid.Children[0].Visibility = Visibility.Hidden;
                        }
                    }
                    else if( ShipSize == 3 )
                    {
                        pr_AmountOfSize3Ships--;
                        pr_AmountOfShips--;
                        if( pr_AmountOfSize3Ships == 0 )
                        {
                            pr_SourceGrid.Children[1].Visibility = Visibility.Hidden;
                        }
                    }
                    else if( ShipSize == 2 )
                    {
                        pr_AmountOfSize2Ships--;
                        pr_AmountOfShips--;
                        if (pr_AmountOfSize2Ships == 0)
                        {
                            pr_SourceGrid.Children[2].Visibility = Visibility.Hidden;
                        }
                    }
                    else if( ShipSize == 1 )
                    {
                        pr_AmountOfSize1Ships--;
                        pr_AmountOfShips--;
                        if (pr_AmountOfSize1Ships == 0)
                        {
                            pr_SourceGrid.Children[3].Visibility = Visibility.Hidden;
                        }
                    }

                    pr_ShipList.Add(new TestShip(FieldIndex, pr_DirectionChange, ShipSize, Ship));
                }
                else
                {
                    //MessageBox.Show("Schiff kann nicht auf dieses Feld gesetzt werden!");
                }
            }
        }

        private void F_PreviewShipOn(int FieldIndex, UIElement Ship, Canvas BattleField, List<Field> GameField)
        {
            if (pr_ModifyShip == true)
            {
                bool ShipDirection = F_getShipDirection(Ship);

                if (GameField.ElementAt(FieldIndex).pu_OpenSpace)
                {
                    double XCoordinate = 0;
                    double YCoordinate = 0;

                    if (ShipDirection == false)
                    {
                        XCoordinate = GameField.ElementAt(FieldIndex).pu_LowerXBarrier + (3);
                        YCoordinate = (GameField.ElementAt(FieldIndex).pu_LowerYBarrier + GameField.ElementAt(FieldIndex).pu_UpperYBarrier) / 2;
                        YCoordinate = YCoordinate - 10;
                    }
                    else
                    {
                        XCoordinate = (GameField.ElementAt(FieldIndex).pu_LowerXBarrier + GameField.ElementAt(FieldIndex).pu_UpperXBarrier) / 2;
                        XCoordinate = XCoordinate - 10;
                        YCoordinate = GameField.ElementAt(FieldIndex).pu_LowerYBarrier + (3);
                    }

                    Canvas.SetLeft(Ship, XCoordinate);
                    Canvas.SetTop(Ship, YCoordinate);

                    BattleField.Children.Add(Ship);
                }
                else
                {
                    //MessageBox.Show("Schiff kann nicht auf dieses Feld gesetzt werden!");
                }
            }
            else
            {
                if (GameField.ElementAt(FieldIndex).pu_OpenSpace)
                {
                    double XCoordinate = 0;
                    double YCoordinate = 0;

                    if (pr_DirectionChange == false)
                    {
                        XCoordinate = GameField.ElementAt(FieldIndex).pu_LowerXBarrier + (3);
                        YCoordinate = (GameField.ElementAt(FieldIndex).pu_LowerYBarrier + GameField.ElementAt(FieldIndex).pu_UpperYBarrier) / 2;
                        YCoordinate = YCoordinate - 10;
                    }
                    else
                    {
                        XCoordinate = (GameField.ElementAt(FieldIndex).pu_LowerXBarrier + GameField.ElementAt(FieldIndex).pu_UpperXBarrier) / 2;
                        XCoordinate = XCoordinate - 10;
                        YCoordinate = GameField.ElementAt(FieldIndex).pu_LowerYBarrier + (3);
                    }

                    Canvas.SetLeft(Ship, XCoordinate);
                    Canvas.SetTop(Ship, YCoordinate);

                    BattleField.Children.Add(Ship);
                }
                else
                {
                    //MessageBox.Show("Schiff kann nicht auf dieses Feld gesetzt werden!");
                }
            }
        }

        private void F_LockSpace(int FieldIndex, List<Field> GameField)
        {
            GameField.ElementAt(FieldIndex).pu_OpenSpace = false;
        }

        private int F_getShipSize(UIElement Ship)
        {
            Rectangle rShip = (Ship as Rectangle);

            bool rShipDirection = F_getShipDirection(rShip);

            if (rShipDirection == false)
            {
                if ((rShip.Width > 120) && (rShip.Width < 160))
                {
                    return 4;
                }
                else if ((rShip.Width > 80) && (rShip.Width < 120))
                {
                    return 3;
                }
                else if ((rShip.Width > 40) && (rShip.Width < 80))
                {
                    return 2;
                }
                else if ((rShip.Width > 0) && (rShip.Width < 40))
                {
                    return 1;
                }
            }
            else
            {
                if ((rShip.Height > 120) && (rShip.Height < 160))
                {
                    return 4;
                }
                else if ((rShip.Height > 80) && (rShip.Height < 120))
                {
                    return 3;
                }
                else if ((rShip.Height > 40) && (rShip.Height < 80))
                {
                    return 2;
                }
                else if ((rShip.Height > 0) && (rShip.Height < 40))
                {
                    return 1;
                }
            }

            return -1;
        }

        private bool F_getShipDirection(UIElement Ship)
        {
            Rectangle rShip = (Ship as Rectangle);

            if( (rShip.Height > 0) && (rShip.Height < 40) )
            {
                return false;
            }
            else if( (rShip.Width > 0) && (rShip.Width < 40) )
            {
                return true;
            }

            return false;
        }

        private int F_getShipIndex(UIElement Ship)
        {
            for( var lauf = 0 ; lauf < pr_ShipList.Count ; lauf++ )
            {
                if( pr_ShipList[lauf].pr_Element == Ship )
                {
                    return lauf;
                }
            }

            return -1;
        }

        private void F_PlaceAttackOn(int FieldIndex, UIElement Attack, Canvas BattleField, List<Field> GameField)
        {
            if (GameField.ElementAt(FieldIndex).pu_OpenSpace)
            {
                double XCoordinate = 0;
                double YCoordinate = 0;

                XCoordinate = (GameField.ElementAt(FieldIndex).pu_LowerXBarrier + GameField.ElementAt(FieldIndex).pu_UpperXBarrier) / 2;
                YCoordinate = (GameField.ElementAt(FieldIndex).pu_LowerYBarrier + GameField.ElementAt(FieldIndex).pu_UpperYBarrier) / 2;

                XCoordinate = XCoordinate - 20;
                YCoordinate = YCoordinate - 20;

                Canvas.SetLeft(Attack, XCoordinate);
                Canvas.SetTop(Attack, YCoordinate);

                BattleField.Children.Add(Attack);
                F_LockSpace(FieldIndex, GameField);
            }
            else
            {

            }
        }
    }
}