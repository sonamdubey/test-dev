IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCategorySections]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCategorySections]
GO

	
 
-- =============================================
-- Author	:	Sachin Bharti(17th March 2016)
-- Description :	Get campaign category sections
-- =============================================
CREATE PROCEDURE [dbo].[GetCategorySections]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT Id , Name 
	FROM CategorySection(NOLOCK)  ORDER BY Id
END

