   M S B A M L   `   `   `  ����@  <Token, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Unique_passUnique_pass  5   	       Unique_pass.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Unique_pass     5   	     � AllowsTransparency  .  � WindowStartupLocation$ CenterScreen=�5
   	    � WindowStyle$
 None=�5   	   
������  � Title$ Inicio sesión��5   	   $	��400��6   $	��300��6,   .��5      � .+�5   
   �� 
���   � �� CornerRadius$ 20��6%   ���#1D202367   � 5   
    � RowDefinitions 5      �� 5      $��30 �6    6.   �� 5      $��* �6    6-   .+�5      � $��0��6    � ColumnDefinitions 5      �� 5      $��* �6'   63   �� 5      $
��auto �6'   66   �� 5      $
��auto �6'   66   .+�5"      �� $��0��6   $��Center=�6,   $��10,0,0,0q�6G   6[   �� 5#      -   5        $��0��5#      $��30(�6)   $��Right=�67   $��-��6S   $��30��6_   
������ 
����� �
������ ��5$      �� 5%      .R�5&      ��  ��Name$ border��5'   %   
������ ._�5+   "   �� $��Center=�5,   )   $��Center=�5-   )   $��-��5.   )   5/       �� Triggers 50      P� 51   "   	 P� Property	 �@J�IsMouseOver
 P� Value
 . . �52   &   ��  �� 
TargetName$ border��6-    �� Property � �� �� Value �#1D20236W   53   #   55      57      �� 58      -   5        $��0,0,5,0q�58      $��1��6*   $��20(�6:   $��X��6H   $��30��6T   
������ 
����� �
������ ��59      �� 5:      .R�5;      �� $ border��5<      
������ $ 0,10,0,0��5>      ._�5A   "   �� $��Center=�5B   !   $��Center=�5C   !   $��X��5D   !   5E       5F      P� 5G   "   	 �@J�IsMouseOver
 . . �5H   &   �� $ border��6-    � �� �#1D20236W   5I   #   5K      5M      5Q      �� 5S      $��1��6   $��	0,100,0,0q�6&   .+�5U      �� $��Center=�6   $��Center=�65   $��Tahoma)�6P   $��20(�6d   
�������.��6�   Ingrese su contraseña unica6�   �� 5W      
�������$��1q�6-   $ 10��6A   $��40��6S   $	��250��6_   $
��10,0q�6k   ._�5X      G� -   5        $��Unique_passG��5X   5   
�������$��Center=�6I   $��Tahoma)�6k   $��20(�6   
������ $��0q�5Y   .   $��5q�6B   5Z      �� 5\      -   5        
������ $
��0,60q�5\   3   $��Ingresar��6A   $��Tahoma)�6T   $��20(�6h   $��40��6v   $	��150��6�   ��5]      �� 5^      .R�5_      �� $ border��5`   !   
�������$ 5��5b   !   ._�5d   "   �� $��Center=�5e   !   $��Center=�5f   !   $��Ingresar��5g   !   5h       5i      P� 5j   "   	 �@J�IsMouseOver
 . . �5k   &   �� $ border��6-    � �� �#2125296W   5l   #   5n      5p      5r      5t      5x      5y      