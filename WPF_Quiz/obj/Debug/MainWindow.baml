   M S B A M L   `   `   `  ����C  ?WPF_Quiz, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:WPF_QuizWPF_Quiz  5   	       WPF_Quiz.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:WPF_Quiz     5   	     � Title$  Form1��5   	   $	��450��6   $	��800��6$   ���#0e1013
�������$	��300��6X   $	��600��6h   .��5
      (� .��5   
   � $
��Auto��6   .+�5      �� .+�5      �� 
�������$��15(�6+   $ ��Adja meg a teljes nevét: ��69   6_   �� 5      -   5         ��Name$ 
Name_input��5       �� 
IsEditable . $��Stretch=�6A   
���   �$
��Bold"�6r   6�   �� 5      
�������$��15(�6+   $��Adja meg a témakört: ��69   6[   � 5      ���#101316.+�5      	� -   5         	�Name$ category1_button��5   "   
�������$��Informatika��6O   $��10,5,0,0q�6e    d� 	IsChecked$
 TrueY�6w   	� 5      -   5        $ category2_button��5   "   
�������$��
Edzőterem��6O   $��	10,25,0,0q�6c   	� 5      -   5        $ category3_button��5   "   
�������$��Gaming��6O   $��	10,45,0,5q�6`   5      �� 5      
�������$��15(�6+   $&�� Adja meg a kérdések számát: ��69   6b   �� 5      -   5         ��Name$ QuestionNumberSlider��5      $
��Left=�67   $	��Top=�6S   $	��120��6k    � Minimum$ 3Q�6w    � Maximum$ 15Q�6�    �� TickPlacement$ BottomRight=�6�   	 �� IsSnapToTickEnabled	 . 
 �� TickFrequency$
 3Q�6�    �� AutoToolTipPlacement$ TopLeft=�6�    �� AutoToolTipPrecision$ 0��6�   �� 5      $$��Több válaszos kérdések is?��6   .��5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      	� -   5        $��MultipleAnswer_YesButon��5   9   
�������$
��Igen��6X   $��	25,0,50,0q�6g   6z   	� 5      -   5        $��MultipleAnswer_NoButon��5   9   
�������$	��Nem��6W   6e   5      5      �� 5       -   5         ��Name$ GameStart_Button��5    +   {�.  $��Játék indítása��6W   ���#132226
�������$��80��6�   $��0, 25, 0, 10q�6�   $��Center=�6�   5!       � 5"      -	   5          �Name$ 	MainFrame��5"      $��-83,0,-83,0q�6'   5#      5$      5%      