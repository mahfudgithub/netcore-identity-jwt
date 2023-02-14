USE [COLLAGE_DB]
GO
/****** Object:  StoredProcedure [dbo].[GetMessageList]    Script Date: 2/14/2023 10:36:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		wot.mahfudin
-- =============================================
ALTER PROCEDURE [dbo].[GetMessageList]
	-- Add the parameters for the stored procedure here	
	@rowStart AS INT = NULL
   ,@rowEnd AS INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	select * from (
		SELECT ROW_NUMBER() over(order by [MSG_CD] asc ) SEQ,
			[MSG_CD],
			[MSG_TEXT] 
		FROM [dbo].[Message]
	)tb 
		where 1 = 1 
		and tb.SEQ between @rowStart and @rowEnd
END
