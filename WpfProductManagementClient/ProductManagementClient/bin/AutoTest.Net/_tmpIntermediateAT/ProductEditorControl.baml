   M S B A M L   `   `   `  ����p  lPloeh.Samples.ProductManagement.PresentationLogic.Wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null��clr-namespace:Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;assembly=Ploeh.Samples.ProductManagement.PresentationLogic.Wpf5Ploeh.Samples.ProductManagement.PresentationLogic.Wpf  5       ProductManagementClientD   >Ploeh.Samples.ProductManagement.WpfClient.ProductEditorControl   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      �pm�clr-namespace:Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;assembly=Ploeh.Samples.ProductManagement.PresentationLogic.Wpf     5      .��5      ]� $��10q�6   .+�5   
   � $��Bottom=�6     � ColumnDefinitions  5      �� 5      $��* �6#   �� 5	      $
��Auto �6#   �� 5
      $
��Auto �6#   .+�5      �� $��1��6   $��0��6%   $��60��62    �� 	IsDefault . P   JPloeh.Samples.ProductManagement.PresentationLogic.Wpf.PresentationCommands   Accept#��Z 6q   {�6N   ��  �� Path$ IsValid�.��6�   _OK6�   �� 5      $��2��6   $��0��6%   $��60��62    �� IsCancel . .��6M   	_Cancel6V   5      � 5   
    � 	Resources 5       �� DataType-  'System.Windows.Controls.ValidationError��%�   (��       5      *, +)�� 6   "  6   .R�5      �� $��Italic$�6    
���  ��$��Right=�6D   $	��0,1q�6`   ��6m   �� $ ErrorContent�5      5        5      �� 5      $
��Auto �6#   �� 5      �� 5       � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��30 �6    �� 5      $
��Auto �6    �� 5      $��30 �6    �� 5      $
��Auto �6    �� 5       $��30 �6    �� 5!      $��30 �6    .+�5#      �� $��0��6   $��0��6$   $	��Id:��61   $��Right=�6?   �� 5$      $��1��6   $��0��6(   $��2��65   $��Center=�6b   ��6I   �� $ Id�6b   �� 5%      $��0��6   $��2��6$   $��_Name:��61   $��Right=�6B    �� Target 6^   �� 	 �� ElementName$	 nameTextBox���� 5&      -   5        
 ��Name$
 nameTextBox��5&      $��1��6+   $��2��6;   $��2��6H   ��6\   �� $
 Name� �� ValidatesOnDataErrors .  �� UpdateSourceTrigger$ PropertyChanged=��� 5'      $��1��6   $��3��6/   $��2��6<   $
��Left=�6P   ��6k   �� $	 nameTextBox��$% (Validation.Errors).CurrentItem��� 5(      $��0��6   $��4��6$   $��_Price:��61   $��Right=�6C    6_   �� $	 priceTextBox���� 5)      -   5        $
 priceTextBox��5)      $��1��6,   $��4��6<   ��6I   �� $ Price� . $ PropertyChanged=��� 5*      -   5        $
 currencyTextBox��5*      $��2��6/   $��4��6?   ��6L   �� $ Currency� . $ PropertyChanged=��� 5+      $��1��6   $��5��6/   $
��Left=�6<   ��6W   �� $	 priceTextBox��$% (Validation.Errors).CurrentItem��� 5,      $��2��6   $��5��6/   $
��Left=�6<   ��6W   �� $	 currencyTextBox��$% (Validation.Errors).CurrentItem�5-      5.      5/      