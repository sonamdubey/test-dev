IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetReportProblemImage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetReportProblemImage]
GO

	

-- =============================================
-- Author      : Vinay Kumar Prajapati  14th Sep 2015
-- Description : To Get AbSure report problem Images . 
--  EXEC  [AbSure_GetReportProblemImage] 4
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetReportProblemImage]
	@AbsureReportProblemsId  INT  
AS
BEGIN
       -- Avoid extra message 
	   SET NOCOUNT ON

		SELECT RPP.HostUrl+RPP.DirectoryPath+RPP.ImageUrlOriginal AS ImageLink
		FROM Absure_ReportProblemPhotos AS RPP WITH(NOLOCK)
		WHERE RPP.Absure_ReportProblemsId=@AbsureReportProblemsId
END

