   M S B A M L   `   `   `  ����D  @KEKPlayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:KEKPlayer	KEKPlayer  5   	       KEKPlayer.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Z VDevExpress.Mvvm.UI, Version=20.2.0.0, Culture=neutral, PublicKeyToken=382ad28f6ae31f30W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:KEKPlayer     5   	     � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)    � DataContext 5   	   ��    ViewModelLocator �� Source# [   �� Path$ MainViewModel�.��5      �  � RowDefinitions 5   
   �� 5      $��3* �6   �� 5      �� 5      .+�5   
    �   � NavigationUIVisibility$ Hidden=�64   $��0��6T   ��6   �� *MemberTrackPage+6T   �� 5   
   $��1��6   $��Center=�6#   $��10,00,10,00q�6>   .+�5      �� ��6   �� *TimerOfCOmpostion+�� 5       � Minimum$ 0Q�6~    � Value 6   �� *CurrentTrackPosition+ �� Mode$ TwoWay=�	 � Maximum	 6I   �� *CurrentTrackLenght+$ TwoWay=�6~   5      �� 5   
   $��2��6   
 �� Orientation$
 
Horizontal=�6#   $��Center=�6=   .+�5      �� $��
Назад��6   ��6&   �� *PastMusicOAir+�� 5      ��6   �� *ChangeStatusMusikOnAir+��6@   �� *Status+�� 5       $��Вперёд��6   ��6'   �� *NextMusicOAir+�� 5!      ��6   �� *PlayerModeContent+��6;   �� *
PlayerMode+5%      �� 5'   
   $��2��6   $��Right=�6O    �� Orientation$ Vertical=�6k   $	 1Q�6�   $ 0Q�6�    6   �� *MusicVolumeSlider+$ TwoWay=�6�   � 5)   
   $��0��6   $��Right=�6   .+�5*      �� $��0��6   $	��Top=�6"   $��Right=�6:   ��6V   �� *OpenFileDialog+.��5+      �� $��Выбрать трек +��6   5-      �� 5.      $��	0,30 ,0,0q�6?   $
��Auto=�6S   $
��Auto=�6�   ��6   �� *AlbomModels+��5/      �� 50      .R�52      A�  A� Columns$ 3��6*   .+�53      �� $
��Wrap=�6@   ��6(   �� *Title+6@   �� 54      $
��Wrap=�6J   ��6(   �� *CompositionCount+6J   55      59      5<      5=      5@      5A      