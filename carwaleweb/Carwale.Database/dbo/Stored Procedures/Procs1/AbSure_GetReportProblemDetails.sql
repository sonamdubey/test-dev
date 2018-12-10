IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetReportProblemDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetReportProblemDetails]
GO

	

-- =============================================
-- Author:      Vinay Kumar Prajapati  14th Sep 2015
-- Description:    To Get AbSure report problem details and photo link  for sending mail... 
--  EXEC  [AbSure_GetReportProblemDetails] 1
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetReportProblemDetails]
	@AbsureReportProblemsId  INT  
AS
BEGIN

	   DECLARE @MailTo VARCHAR(500) = 'thiyagarajan.ravi@carwale.com,lavraj.ghadigaonkar@carwale.com,shivani.garg@carwale.com,shweta.kumar@carwale.com,puneet.kumar@carwale.com,arun.g@carwale.com,tejashree.p@carwale.com'
	   BEGIN
            SELECT ISNULL(C.Name,'') AS City , ISNULL(AR.Name,'') AS Area ,RP.PhoneManufacturer AS Make ,RP.PhoneModel AS Model,RP.AppVersion,TU.Email,TU.UserName AS SurveyorName,TU.Mobile,RP.IsProblem,RP.Comment,RP.EntryDate,
			CASE ISNULL(RP.IsProblem,0) WHEN 1 THEN 'Report a Problem-Absure App : '+ TU.Mobile ELSE 'Suggestion-Absure App : '+ TU.Mobile END AS Subject,
			@MailTo AS MailTo,
			(SELECT UserName FROM TC_Users  WITH(NOLOCK) WHERE HierId =  TU.HierId.GetAncestor(1) AND BranchId =TU.BranchId) AS Agency,
			@AbsureReportProblemsId AbsureReportProblemsId,RP.ImageCount
			FROM  Absure_ReportProblems AS RP WITH(NOLOCK) 
			LEFT JOIN Cities AS C WITH(NOLOCK) ON C.Id=RP.CityId
			LEFT JOIN Areas AS AR WITH(NOLOCK) ON AR.ID=RP.AreaId
			LEFT JOIN TC_Users AS TU WITH(NOLOCK) ON TU.Id=RP.SurveyorId
			WHERE RP.Absure_ReportProblemsId=@AbsureReportProblemsId

		END

END

