   M S B A M L   `   `   `  ����C  ?PaintApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:PaintAppPaintApp  5   	       PaintApp.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :xctk)http://schemas/xceed.com/wpf/xaml/toolkit     5   	   (localclr-namespace:PaintApp     5   	   -   5          � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)   .��5
      �  � ColumnDefinitions 5   
   �� 5      $	��21* �6   �� 5      $
��139* �6   .+�5   
   �� $��100px��6   $	��450��6#   $
��Left=�60   ���	#FF756C6C$��	0,0,0,-16q�6b   .+�5      �� -   5         ��Name$ btn_Rectangle��5      $��	Rectangle��6,   6@   �� 5      -   5        $ btn_Ellipse��5      $��Ellipse��6*   6<   �� 5      -   5        $
��Line��5      6$   �� 5      -   5        $ btn_Polygon��5      $��Polygon��6*   6<   �� 5      -   5        $ btn_ChangeColor��5      $��Change color��6.   $��	0,100,0,0q�6E   6X   �� 5      -   5        $ btn_Serialize��5      $
��Save��6,   6;   �� 5      -   5        $ btn_Deserialize��5      $
��Load��6.   6=   5      �� 5   
   -	   5         ��Name$ canvas��5      $	��450��6!   
������ $��	0,0,0,-16q�6d   $��Right=�6w   $	��700��6�   $��2��60  .+�5      �� -
   5        $ btn_ChangeSize��5      $�� ��6-   $y�Hidden=�68   $��10��6L   $��10��6W   6c   5      5      5      