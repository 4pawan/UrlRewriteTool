   M S B A M L   `   `   `  ����R  NDigitasLbi.RedirectTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:DigitasLbi.RedirectToolDigitasLbi.RedirectTool  5   	   O,clr-namespace:DigitasLbi.RedirectTool.CommonDigitasLbi.RedirectTool.Common  5   	   U/clr-namespace:DigitasLbi.RedirectTool.ViewModel!DigitasLbi.RedirectTool.ViewModel  6E   -    'DigitasLbi.RedirectTool.View.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:DigitasLbi.RedirectTool     5   	   ?Common,clr-namespace:DigitasLbi.RedirectTool.Common     5   	   E	ViewModel/clr-namespace:DigitasLbi.RedirectTool.ViewModel     6E     � Title$  Url redirect tool��5	   	   $	��450��6#   $	��550��60    � Icon$7 1/DigitasLbi.RedirectTool;component/Assets/lbi.ico��6<    � 	Resources 5
      ;   5DigitasLbi.RedirectTool.Common.StringToColorConverter   StringToColorConverter6   0DigitasLbi.RedirectTool.Common.NegationConvertor  NegationConvertor0   *DigitasLbi.RedirectTool.Common.OrConverter  OrConverterA   ;DigitasLbi.RedirectTool.Common.BooleanToVisibilityConverter   BooleanToVisibilityConverter��%t   &	        &	      &	 &     &	 9       5   
   6(     5   
   6#     5   
   6     5   
   6.   5       � DataContext 5      <   6DigitasLbi.RedirectTool.ViewModel.UtilityMainViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $	��48* �6   �� 5      $	��40* �6   �� 5      $	��38* �6   �� 5      $	��37* �6   �� 5      $	��37* �6   �� 5      $	��37* �6   �� 5      $
��258* �6    5   
    �� 
TargetType �� Property �� Value��%"  '��       �� 5      " ��6   .�5      ��  � ��$
 Left=�68   �� 5        � ��$	 318��6*   �� 5!       � ��$ 23��6+   �� 5"       �@��TextWrapping$
 Wrap=�61   �� 5#       � ��$	 Top=�66   �� 5$       �@��
IsReadOnly . 6/   5%      5&      .+�5'   
   �� -   5        $��	BtnSource��5'      $��Please Select Excel��6#   $
��Left=�6A   $	��Top=�6\   $	��142��6t   $��17,7,0,0q�6�   $��1��6�   ��6�   �� *ShowDialogToSelectExcel+6�   �� 5(   
   $��1��6A   $��	183,7,0,0q�6N   ��6   �� *ExcelSourcePath+ �� Mode$ TwoWay=�6N   �� 5)   
   -   5        $��BtnDestination��5)      $ ��Please select path to save��6(   $
��Left=�6M   $	��Top=�6h   $	��142��6�   $��17,8,0,0q�6�   $��2��6�   ��6�   �� *ShowSaveDialog+6�   �� 5*   
   $��2��6F   $��	183,8,0,0q�6S   ��6   �� *ExcelDestinationPath+$ TwoWay=�6S   �� 5+   
   $��6��6   .��5,      �� ��. $��17,13,41,10q�6>   ��6T   �� *	Message+$ TwoWay=���6y   �� *
StatusFlag+$ TwoWay=�	 �� 	Converter#	 [  5-      �� 5.   
   $��Generate xml��6I   $
��Left=�6`   $	��Top=�6{   $	��484��6�   $��
17,7.4,0,0q�6�   $��3��6�   {�6   �� *IsGenerateXmlEnabled+$ TwoWay=���6�   �� *UrlRewriteUtilityCommand+5=      �� 5>   
   $��Configure xml��6k   $
��Left=�6�   $	��Top=�6�   $	��484��6�   $��
17,7.4,0,0q�6�   $��4��6  {�6   �� *IsConfigXmlEnabled+$ TwoWay=�
 �� UpdateSourceTrigger$
 PropertyChanged=���6�   �� *ConfigureRewriteRuleCommand+6  �� 5?   
   $��Validate xml��6n   $
��Left=�6�   $	��Top=�6�   $	��484��6�   $��
17,7.4,0,0q�6�   $��5��6  {�6   �� *IsValidateXmlEnabled+$ TwoWay=�$
 PropertyChanged=���6�   �� *ValidateRewriteRuleCommand+6  5@      5A      