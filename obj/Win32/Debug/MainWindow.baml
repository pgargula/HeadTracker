   M S B A M L   `   `   `  ����M  IFaceTrackingBasics, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:FaceTrackingBasicsFaceTrackingBasics  5   	   #    FaceTrackingBasics.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   2local clr-namespace:FaceTrackingBasics     5   	   -   5        $��form1��5   "     � Title$  Face Tracking Basics��5   	   $	��440��5   	   $	��640��6   $y�Visible=�6/    � WindowStyle$
 None=�6D    � 
ResizeMode$ NoResize=�6W   $��Bottom=�6m   $��Right=�6�   $��Right=�6�   $��Bottom=�6�    J� Opacity$ 1Q�6�   ��5	      �� 5
   
    �� ImageSource$2 ,/FaceTrackingBasics;component/Images/tlo.jpg��6    � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property	 �� Value��%�   &	        &	 #     &	 F     '�� i     �� 5   
   $ 	#ff6e6e6e��62   �� 5   
   $ 	#ff52318f��64   �� 5   
   $ 	#ff00BCF2��62   �� 5   
   " ��6   .�5      ��  �@J�SnapsToDevicePixels	 . 64   5      5      .��5      � -   5        $��
layoutGrid��5      $��
10 0 10 10q�6   $	��420��61   $	��593��6>   $��Center=�6J   $��Center=�6g   
 � ColumnDefinitions
 5   
   �� 5      $	��10* �6   �� 5      $
��110* �6   �� 5      $
��473* �6   .+�5   
   (� -   5        $��viewBox1��5   A    (� Stretch$ Uniform=�6   $��Center=�6$   $y�Visible=�6Q   $��
0,24,468,0q�6f   $	��125��6z   $��3��6�   $��91��6�   $	��Top=�6�   .��5      � -   5        $��MainGrid��5      $	��640��6#   $	��480��6/   $��0q�6<   
 5      �� 5      $
��168* �6'   �� 5      $	��39* �6'   �� 5       $
��222* �6'   �� 5!      $
��103* �6'   �� 5"      $
��108* �6'   .+�5$      �� -   5        $��
ColorImage��5$      $��5��6*   +   %FaceTrackingBasics.FaceTrackingViewer  5%      -   5          Name$ faceTrackingViewer��5%   +   $��	0,6,-10,0q�6G   $	��476��6Z   $	��Top=�6g   $y�Visible=�6   $��5��6�   5&      5'      �� 5(   
   -   5        $��image1��5(   [   $��1��6   $��65��6    $
��Left=�6,   $��
43,343,0,0q�6G   $
��Fill=�6i   $	��Top=�6x   $	��500��6�   $6��0/FaceTrackingBasics;component/Images/zakoncz.jpg��6�   $��2��61  �� 5)   
   -   5        $��image2��5)   ]   $��2��6   $	��200��6    $
��Left=�6-   $��109,134,0,0q�6H   $
��Fill=�6k   $	��Top=�6z   $	��150��6�   $B��</FaceTrackingBasics;component/bin/x86/Debug/images/opcje.jpg��6�   6"  �� 5*   
   -	   5        $��image3��5*   ]   $��2��6   $	��200��6    $
��Left=�6-   $��283,134,0,0q�6H   $F��@/FaceTrackingBasics;component/bin/x86/Debug/images/aplikacje.jpg��6k   $
��Fill=�6�   $	��Top=�6�   $	��150��6�   6&  �� 5+   
   -
   5        $��image4��5+   p   $��1��6   $��2��6    $	��200��64   $
��Left=�6A   $��
44,134,0,0q�6\   $G��A/FaceTrackingBasics;component/bin/x86/Debug/images/kalibracja.jpg��6~   $
��Fill=�6�   $	��Top=�6�   $	��150��6�   6:  5,      5-      