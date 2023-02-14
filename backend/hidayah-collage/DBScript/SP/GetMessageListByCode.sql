USE [COLLAGE_DB]
GO
/****** Object:  StoredProcedure [dbo].[GetMessageListByCode]    Script Date: 2/14/2023 10:37:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		wot.mahfudin
-- =============================================
ALTER PROCEDURE [dbo].[GetMessageListByCode]
	-- Add the parameters for the stored procedure here	
	@msgCode AS VARCHAR(10) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	select * from (
		SELECT ROW_NUMBER() over(order by [MSG_CD] asc ) SEQ,
			[MSG_CD],
			[MSG_TEXT] 
		FROM [dbo].[Message]
		where 1=1
		AND (nullif(@msgCode,'') is null or MSG_CD like '%'+@msgCode+'%')
	)tb 
		where 1 = 1 
		--and tb.SEQ between 1 and 5
END
