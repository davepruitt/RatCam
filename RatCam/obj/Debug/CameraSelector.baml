   M S B A M L   `   `   `  ����A  =RatCam, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:RatCamRatCam  5   	       RatCam.CameraSelector   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:RatCam     5   	     � Title$  Please select a camera��5   	    � SizeToContent$ Height=�5	   	    � 
ResizeMode$ NoResize=�5
   	    � WindowStartupLocation$ CenterOwner=�5   	   $	��350��5   	   .��5      ��  �� Orientation$ Vertical=�6   .+�5   
   �� $��1q�5      $��5q�5      ��5      ��  �� Path$ AvailableCameras� �� SelectedIndex 5      �� $ SelectedCameraIndex���5      �� 5      .R�5      �� $ Vertical=�6!   .+�5      �� $��24(�6Y   ��6$   �� $ 	BoothName� �� FallbackValue$ Booth��6Y   �� 5      $ 
Horizontal=�6%   .+�5      �� $��10(�6P   ��6(   �� $ DeviceInformation�6P   5      5      5      5      �� 5   
   $ 
Horizontal=�6   $��Center=�6.   $��10q�6K   .+�5      �� -   5        $��RefreshButton��5      $	��120��6*   $��40��66   $��5,0,0,5q�6B   $��Bottom=�6o   ��5       �� 5!      .+�5"      F� 
��� � �$��3��6.   ��5#      B� 5$   "   .��5%   &   E�  E� 
StartPoint$
 10,0&�61   .��5&   *   �� 	 �� 
IsLargeArc	 . 
 �� SweepDirection$
 	Clockwise=�6G    �� Size$ 10,10��6b    �� Point$
 9,-5&�6o   5'   '   5(   #   5*      � 5+       � Points' �   ���  ��� 	= �����ا ��v�
��� � �
��� � �6K   5,      5.      �� 5/      -   5        $��OK��5/      $��Center=�6"   $��Bottom=�6?   $	��120��6Z   $��40��6f   $��5,0,0,5q�6r   $��18(�50      $
��Bold"�6#   51      52      53      54      