   M S B A M L   `   `   `  ����G  CEasyJunction, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Zumwani.EasyJunctionZumwani.EasyJunction  5   	   (    "Zumwani.EasyJunction.InstallWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   4local"clr-namespace:Zumwani.EasyJunction     5   	   -   5        $	��220��5   	   $	��550��6     � 
ResizeMode$  NoResize=�6"    � ShowInTaskbar .   � WindowStartupLocation$ CenterOwner=�5       � Icon$ ../Assets/icon.ico��5   	   .��5
      � .+�5   
   � $��60q�6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6     � ColumnDefinitions 5      �� 5      �� 5      $
��Auto �6#   .+�5      �� $ ��Choose path to install to:��6   �� 5      -   5         ��Name$ PathBox��5      $��Stretch=�6'   $��1��6E   $��5q�6R   $	��Top=�6^   �� 5      -   5         ��Name$ BrowseButton��5      $��	Browse...��6+   $��5,0,0,0q�6?   $
��15,3q�6P   $��Right=�6_   $��1��6{   $��1��6�   6�   5      �� 5   
   -   5        $ 
DoneButton��5      $��10q�6>   $��Right=�6J   $��Bottom=�6f   .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� -   5        	 ��Name$	 UACImage��5   [   $#��../Assets/UAC-Shield-Icon.png��6   $��16��6?   $��4,0,0,0q�6J   6[   �� 5      $��Install��6   $	��5,5q�6+   5       5!      5#      5%      