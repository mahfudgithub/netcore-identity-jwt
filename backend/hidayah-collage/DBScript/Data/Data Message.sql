USE [COLLAGE_DB]
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR001', N'Email already exist')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR002', N'There is no user with that Email address')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR003', N'Invalid Password.')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR004', N'Password and Confirm Password do not match.')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR005', N'Invalid refresh token')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR006', N'Not Authorize')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR007', N'Data Already Exists')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'ERR008', N'No Data Found')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC001', N'Register Successfully')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC002', N'Forgot Password Success')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC003', N'Login Successfully')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC004', N'Logout Successfully')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC005', N'Success Refresh Token')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC006', N'Successfully Retrieve Data')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC007', N'Reset Password Successfully.')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC008', N'Update Data Successfully')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC009', N'Delete Data Successfully')
GO
INSERT [dbo].[Message] ([MSG_CD], [MSG_TEXT]) VALUES (N'SUC010', N'Insert Data Successfully')
GO
