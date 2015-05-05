﻿#pragma checksum "..\..\..\UserControls\uc_GameInterface.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "188683DFF2B0A86BF2B1222E8445BF91"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SV_Client.Graphic;
using SV_Client.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SV_Client.UserControls {
    
    
    /// <summary>
    /// uc_GameInterface
    /// </summary>
    public partial class uc_GameInterface : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 42 "..\..\..\UserControls\uc_GameInterface.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas ShipS4;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\UserControls\uc_GameInterface.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas ShipS3;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\UserControls\uc_GameInterface.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas ShipS2;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\UserControls\uc_GameInterface.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas ShipS1;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\UserControls\uc_GameInterface.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas UpperGameField;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\..\UserControls\uc_GameInterface.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas LowerGameField;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SV_Client;component/usercontrols/uc_gameinterface.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\uc_GameInterface.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 10 "..\..\..\UserControls\uc_GameInterface.xaml"
            ((SV_Client.UserControls.uc_GameInterface)(target)).SizeChanged += new System.Windows.SizeChangedEventHandler(this.F_ScaleInfoUpdate);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 34 "..\..\..\UserControls\uc_GameInterface.xaml"
            ((System.Windows.Controls.Grid)(target)).PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ViewList_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\UserControls\uc_GameInterface.xaml"
            ((System.Windows.Controls.Grid)(target)).PreviewMouseMove += new System.Windows.Input.MouseEventHandler(this.ViewList_PreviewMouseMove);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ShipS4 = ((System.Windows.Controls.Canvas)(target));
            return;
            case 4:
            this.ShipS3 = ((System.Windows.Controls.Canvas)(target));
            return;
            case 5:
            this.ShipS2 = ((System.Windows.Controls.Canvas)(target));
            return;
            case 6:
            this.ShipS1 = ((System.Windows.Controls.Canvas)(target));
            return;
            case 7:
            
            #line 80 "..\..\..\UserControls\uc_GameInterface.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ReadyClick);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 81 "..\..\..\UserControls\uc_GameInterface.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SurrenderClick);
            
            #line default
            #line hidden
            return;
            case 9:
            this.UpperGameField = ((System.Windows.Controls.Canvas)(target));
            
            #line 106 "..\..\..\UserControls\uc_GameInterface.xaml"
            this.UpperGameField.DragEnter += new System.Windows.DragEventHandler(this.OwnGameField_DragEnter);
            
            #line default
            #line hidden
            
            #line 106 "..\..\..\UserControls\uc_GameInterface.xaml"
            this.UpperGameField.Drop += new System.Windows.DragEventHandler(this.OwnGameField_Drop);
            
            #line default
            #line hidden
            
            #line 106 "..\..\..\UserControls\uc_GameInterface.xaml"
            this.UpperGameField.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.OwnGameField_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 106 "..\..\..\UserControls\uc_GameInterface.xaml"
            this.UpperGameField.PreviewMouseMove += new System.Windows.Input.MouseEventHandler(this.OwnGameField_PreviewMouseMove);
            
            #line default
            #line hidden
            
            #line 106 "..\..\..\UserControls\uc_GameInterface.xaml"
            this.UpperGameField.DragOver += new System.Windows.DragEventHandler(this.OwnGameField_DragOver);
            
            #line default
            #line hidden
            return;
            case 10:
            this.LowerGameField = ((System.Windows.Controls.Canvas)(target));
            
            #line 133 "..\..\..\UserControls\uc_GameInterface.xaml"
            this.LowerGameField.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.OpponentGamefieldAttack);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

