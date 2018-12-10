IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewUpcomingModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewUpcomingModels]
GO

	   
-- =============================================
-- Author	:	Sachin Bharti on 21st March 2016
-- Description	:	Get new and upcoming models excluding discontinued
-- =============================================
CREATE PROCEDURE [dbo].[GetNewUpcomingModels]
	@MakeId	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT  CM.ID AS Value , CM.Name AS Text
	FROM CarModels CM(NOLOCK)
	 WHERE
	(CM.Futuristic =1 OR CM.New =1) 
	AND CM.IsDeleted = 0
	AND CM.CarMakeId = @MakeId
	ORDER BY CM.Name
END
