IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_CommonFeedbackReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_CommonFeedbackReport]
GO

	
/*
		Author: Amit Kumar(24 th april 2013)
		Summary: Used to create common report
		Modifier: 1. Amit Kumar(8th may 2013)(added dealer constrain)
				  2. Amit Kumar (30th may 2013)(Added mobile no of customer) 
		
*/
CREATE PROCEDURE [dbo].[CRM_CommonFeedbackReport]
@feedbackType   INT,
@toDate			DATETIME,
@frmDate		DATETIME,
@makeId			NUMERIC(18,0),
@dealerId		NUMERIC(18,0),
@type			SMALLINT			
AS 
BEGIN 
	IF(@type=1)
		BEGIN
			SELECT CFQ.Id AS QuestionId ,CFQ.Question ,CFQ.AnswerType,CFQ.Alias, CF.AnswerId,CFA.Answer, CC.Mobile AS CustomerMobile,
				CF.CBDId,CF.Comments,CF.FBDate,MMV.Make,MMV.MakeId,CL.ID AS LeadId,CDA.Id As DealerId,ND.Name AS DealerName,
				CC.FirstName CustomerName,CC.ID AS CustId,MMV.Car AS CarName,OU.UserName AS UpdatedBy, CDA.CreatedOn LeadAssignedDate,
				(CASE CLS.CategoryId WHEN 3 THEN LA.Organization ELSE 'CarWale' END) AS SourceName,C.Name AS CityName,S.Name AS State
			FROM CRM_FeedbackQuestions CFQ (NOLOCK) 
				INNER JOIN CRM_Feedback CF (NOLOCK) ON CFQ.Id = CF.QuestionId AND CFQ.IsActive = 1 
				LEFT JOIN CRM_FeedbackAnswers CFA (NOLOCK) ON CFA.Id = CF.AnswerId
				INNER JOIN CRM_CarBasicData CBD (NOLOCK) ON CF.CBDId = CBD.ID  
				LEFT JOIN CRM_CarDealerAssignment CDA (NOLOCK) ON CDA.CBDId = CBD.ID AND CBD.IsDealerAssigned = 1
				LEFT JOIN NCS_Dealers ND (NOLOCK) ON ND.ID = CDA.DealerId
				LEFT JOIN CRM.vwMMV MMV (NOLOCK) ON MMV.VersionId = CBD.VersionId 
				INNER JOIN CRM_Leads CL (NOLOCK) ON CL.ID = CBD.LeadId
				INNER JOIN CRM_Customers CC (NOLOCK) ON CL.CNS_CustId = CC.ID
				INNER JOIN Cities C (NOLOCK) ON C.ID = CC.CityId
				INNER JOIN States S (NOLOCK) ON C.StateId = S.ID
				INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = CF.UpdatedBy
				LEFT JOIN CRM_LeadSource CLS (NOLOCK) ON CLS.LeadId = CL.ID
				LEFT JOIN LA_Agencies AS LA (NOLOCK) ON CLS.SourceId = LA.Id
			WHERE   CFQ.FeedbackType = @feedbackType  AND  (@makeId IS NULL OR MMV.MakeId = @makeId ) 
				AND  (@dealerId IS NULL OR CDA.DealerId = @dealerId )
				AND CONVERT(DATE,CF.FBDate)BETWEEN CONVERT(DATE,@frmDate) AND CONVERT(DATE,@toDate)
		END
	IF(@type=2)
	BEGIN
		SELECT CFQ.Id AS QuestionId ,CFQ.Question ,CFQ.AnswerType,CFQ.Alias, CF.AnswerId,CFA.Answer,  CC.Mobile AS CustomerMobile,
				CF.CBDId,CF.Comments,CF.FBDate,MMV.Make,MMV.MakeId,CL.ID AS LeadId,CDA.Id As DealerId,ND.Name AS DealerName,
				CC.FirstName CustomerName,CC.ID AS CustId,MMV.Car AS CarName,OU.UserName AS UpdatedBy,CDA.CreatedOn LeadAssignedDate, 
				(CASE CLS.CategoryId WHEN 3 THEN LA.Organization ELSE 'CarWale' END) AS SourceName,C.Name AS CityName,S.Name AS State
			FROM CRM_FeedbackQuestions CFQ (NOLOCK) 
				INNER JOIN CRM_Feedback CF (NOLOCK) ON CFQ.Id = CF.QuestionId AND CFQ.IsActive = 1 
				LEFT JOIN CRM_FeedbackAnswers CFA (NOLOCK) ON CFA.Id = CF.AnswerId
				INNER JOIN CRM_CarBasicData CBD (NOLOCK) ON CF.CBDId = CBD.ID  
				INNER JOIN CRM_CarDealerAssignment CDA (NOLOCK) ON CDA.CBDId = CBD.ID AND CBD.IsDealerAssigned = 1
				INNER JOIN NCS_Dealers ND (NOLOCK) ON ND.ID = CDA.DealerId
				INNER JOIN CRM.vwMMV MMV (NOLOCK) ON MMV.VersionId = CBD.VersionId 
				INNER JOIN CRM_Leads CL (NOLOCK) ON CL.ID = CBD.LeadId
				INNER JOIN CRM_Customers CC (NOLOCK) ON CL.CNS_CustId = CC.ID
				INNER JOIN Cities C (NOLOCK) ON C.ID = CC.CityId
				INNER JOIN States S (NOLOCK) ON C.StateId = S.ID
				INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = CF.UpdatedBy
				LEFT JOIN CRM_LeadSource CLS (NOLOCK) ON CLS.LeadId = CL.ID
				LEFT JOIN LA_Agencies AS LA (NOLOCK) ON CLS.SourceId = LA.Id
				
			WHERE   CFQ.FeedbackType = @feedbackType  AND  (@makeId IS NULL OR MMV.MakeId = @makeId ) 
				AND  (@dealerId IS NULL OR CDA.DealerId = @dealerId )
				AND CONVERT(DATE,CDA.CreatedOn)BETWEEN CONVERT(DATE,@frmDate) AND CONVERT(DATE,@toDate)
	END

END