USE [COLLAGE_DB]
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR001', N'Email already exist')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR002', N'There is no user with that Email address')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR003', N'Invalid Password.')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR004', N'Password and Confirm Password do not match.')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR005', N'Invalid refresh token')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR006', N'Not Authorize')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR007', N'Data Already Exists')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR008', N'No Data Found')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC001', N'Register Successfully')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC002', N'Forgot Password Success')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC003', N'Login Successfully')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC004', N'Logout Successfully')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC005', N'Success Refresh Token')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC006', N'Successfully Retrieve Data')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC007', N'Reset Password Successfully.')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC008', N'Update Data Successfully')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC009', N'Delete Data Successfully')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC010', N'Insert Data Successfully')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC012', N'Get Data Successfully')
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC013', N'Email has been sent')
GO
