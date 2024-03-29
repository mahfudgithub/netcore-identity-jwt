USE [COLLAGE_DB]
GO
/****** Object:  StoredProcedure [dbo].[GetSystemByType]    Script Date: 3/30/2023 9:57:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		wot.mahfudin
-- =============================================
CREATE PROCEDURE [dbo].[GetSystemByType]
	-- Add the parameters for the stored procedure here	
	@type AS VARCHAR(450) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	SELECT  [Type]
			,[Code]
			,[Value_Txt]
			,[CreatedBy]
			,[CreatedDt]
	FROM [dbo].[System] 
	where 1 = 1 
	and [Type] = @type
END
GO
