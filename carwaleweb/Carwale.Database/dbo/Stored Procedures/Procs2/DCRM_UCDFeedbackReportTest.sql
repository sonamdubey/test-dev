IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UCDFeedbackReportTest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UCDFeedbackReportTest]
GO

	CREATE PROCEDURE [dbo].[DCRM_UCDFeedbackReportTest]

/*
		Author: Amit Kumar(29 th april 2013)
		Summary: Used to create UCD report
		Modifier: 1. Amit Kumar on 7th may 2013(added username)
		Modifier: 2. Amit Kumar on 15th may 2013( added city name and state)
		
*/
@feedbackType   INT,
@toDate			DATETIME = NULL,
@frmDate		DATETIME = NULL,
@InqryFrDate	DATETIME = NULL,
@InqryToDate	DATETIME = NULL,
@dealerId		NUMERIC,
@Type			INT 		
AS 
BEGIN 
	--When user apply ocnstraint on Inquiries Date
	IF @Type = 1
		BEGIN
			SELECT  CFQ.Id AS QuestionId ,CFQ.Question ,CFQ.AnswerType,CFQ.Alias,CFA.Answer,DUF.DealerId,
				DUF.AnswerId,DUF.CustomerId,DUF.Comments,DUF.FBSubmitDate AS FBDate,UCP.RequestDateTime ,D.Organization,UCP.CarModelNames,
				C.Name AS CustomerName,C.ID AS CustId, OU.UserName,ISNULL(CC.Name,'') AS CityName,ISNULL(S.Name,'') AS State 
			FROM CRM_FeedbackQuestions CFQ (NOLOCK) 
				INNER JOIN DCRM_UCDFeedback DUF (NOLOCK) ON CFQ.Id = DUF.QuestionId AND CFQ.IsActive = 1 
				INNER JOIN CRM_FeedbackAnswers CFA (NOLOCK) ON CFA.Id = DUF.AnswerId
				INNER JOIN UsedCarPurchaseInquiries UCP (NOLOCK) ON UCP.CustomerID = DUF.CustomerId  
					AND ((CONVERT(DATE,UCP.RequestDateTime) BETWEEN  CONVERT(DATE,@InqryFrDate) 
					AND  CONVERT(DATE,@InqryToDate)) OR ( @InqryFrDate IS NULL  AND @InqryToDate IS NULL ))
				INNER JOIN Customers  C (NOLOCK) ON DUF.CustomerId =  C.Id
				INNER JOIN Dealers D (NOLOCK) ON D.ID = DUF.DealerId
				INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = DUF.UpdatedBy
				LEFT JOIN Cities CC (NOLOCK) ON CC.ID = C.CityId
				LEFT JOIN States S (NOLOCK) ON CC.StateId = S.ID
			WHERE   CFQ.FeedbackType = @feedbackType AND (@dealerId IS NULL OR DUF.DealerId = @dealerId) 
		END
	--If user apply constraint on FeedBack taken date
	ELSE IF @Type = 2
		BEGIN
			SELECT   DISTINCT CFQ.Id AS QuestionId ,CFQ.Question ,CFQ.AnswerType,CFQ.Alias,CFA.Answer,DUF.DealerId,
			DUF.AnswerId,DUF.CustomerId,DUF.Comments,DUF.FBSubmitDate AS FBDate,D.Organization,
			C.Name AS CustomerName,C.ID AS CustId, OU.UserName,ISNULL(CC.Name,'') AS CityName,ISNULL(S.Name,'') AS State,UCP.CarModelNames,
			UCP.RequestDateTime,DUF.FBSubmitDate,DCC.ActionTakenOn
			FROM CRM_FeedbackQuestions CFQ (NOLOCK) 
			INNER JOIN DCRM_UCDFeedback DUF (NOLOCK) ON CFQ.Id = DUF.QuestionId AND CFQ.IsActive = 1 
			INNER JOIN DCRM_CustomerCalling DCC (NOLOCK) ON DCC.CustomerId = DUF.CustomerId
				AND CONVERT(DATE,DCC.ActionTakenOn ) = CONVERT(DATE,DUF.FBSubmitDate)
			INNER JOIN UsedCarPurchaseInquiries UCP (NOLOCK) ON UCP.CustomerID = DUF.CustomerId  
			INNER JOIN CRM_FeedbackAnswers CFA (NOLOCK) ON CFA.Id = DUF.AnswerId
			INNER JOIN Customers  C (NOLOCK) ON DUF.CustomerId =  C.Id
			INNER JOIN Dealers D (NOLOCK) ON D.ID = DUF.DealerId
			INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = DUF.UpdatedBy
			LEFT JOIN Cities CC (NOLOCK) ON CC.ID = C.CityId
			LEFT JOIN States S (NOLOCK) ON CC.StateId = S.ID
			WHERE   CFQ.FeedbackType = 4 --AND DUF.CustomerId = 9
				AND  (CONVERT(DATE,DUF.FBSubmitDate) BETWEEN  CONVERT(DATE,@frmDate) AND  CONVERT(DATE,@toDate)) 
		END
END