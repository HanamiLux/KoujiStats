﻿#pragma checksum "..\..\..\AddEditResourcePage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "500D7CBC4E95C52C8EA48CA75982656D6130B185"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
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
using SystemConstruction;


namespace SystemConstruction {
    
    
    /// <summary>
    /// AddEditResourcePage
    /// </summary>
    public partial class AddEditResourcePage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 29 "..\..\..\AddEditResourcePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ResourceActionTB;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\AddEditResourcePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ResourceNameTB;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\AddEditResourcePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ResourceCountTB;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\..\AddEditResourcePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ResourcePricePerOneTB;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\AddEditResourcePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox BuildingComboBox;
        
        #line default
        #line hidden
        
        
        #line 131 "..\..\..\AddEditResourcePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnAddEditResource;
        
        #line default
        #line hidden
        
        
        #line 139 "..\..\..\AddEditResourcePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock AddEditTB;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\..\AddEditResourcePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BackBtn;
        
        #line default
        #line hidden
        
        
        #line 155 "..\..\..\AddEditResourcePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame ViewResourceFrame;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SystemConstruction;component/addeditresourcepage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\AddEditResourcePage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ResourceActionTB = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.ResourceNameTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.ResourceCountTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.ResourcePricePerOneTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.BuildingComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.BtnAddEditResource = ((System.Windows.Controls.Button)(target));
            
            #line 138 "..\..\..\AddEditResourcePage.xaml"
            this.BtnAddEditResource.Click += new System.Windows.RoutedEventHandler(this.BtnAddEditResource_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.AddEditTB = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.BackBtn = ((System.Windows.Controls.Button)(target));
            
            #line 151 "..\..\..\AddEditResourcePage.xaml"
            this.BackBtn.Click += new System.Windows.RoutedEventHandler(this.BackBtn_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.ViewResourceFrame = ((System.Windows.Controls.Frame)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 4:
            
            #line 78 "..\..\..\AddEditResourcePage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.PlusResourceBtn_Click);
            
            #line default
            #line hidden
            break;
            case 5:
            
            #line 90 "..\..\..\AddEditResourcePage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.MinusResourceBtn_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
