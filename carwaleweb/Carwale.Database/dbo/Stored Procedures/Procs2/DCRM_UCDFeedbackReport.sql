IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UCDFeedbackReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UCDFeedbackReport]
GO

	CREATE PROCEDURE [dbo].[DCRM_UCDFeedbackReport]

/*
		Author: Amit Kumar(29 th april 2013)
		Summary: Used to create UCD report
		Modifier: 1. Amit Kumar on 7th may 2013(added username)
		Modifier: 2. Amit Kumar on 15th may 2013( added city name and state)
		Modifier: 3. Sachin Bharti on 28th May 2013 (Apply constraint on Feedback and Inquiery date )
		Modifier: 4. Amit Kumar on 17th sept 2013 ( madi it feedback based on inquiry date only and IsFeedbackGiven based )
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
	--When user apply constraint on Inquiries Date
	DECLARE @isFeedbackTaken smallint

	IF(@Type=2)
		SET @isFeedbackTaken=1
	IF(@Type=3)
		SET @isFeedbackTaken=0


	IF (@Type = 2 OR @Type = 3)--feedbackGiven
		BEGIN
			WITH ABC AS(
			SELECT  DISTINCT CFQ.Id AS QuestionId ,CFQ.Question ,CFQ.AnswerType,CFQ.Alias,CFA.Answer,DUF.DealerId,
				DUF.AnswerId,DUF.CustomerId,DUF.Comments,CONVERT(VARCHAR, DUF.FBSubmitDate,106) AS FBDate,'' AS RequestDateTime ,D.Organization,'' AS CarModelNames,
				C.Name AS CustomerName,C.ID AS CustId, OU.UserName,ISNULL(CC.Name,'') AS CityName,ISNULL(S.Name,'') AS State ,C.Mobile,DCC.InquiryDate
				
			FROM CRM_FeedbackQuestions CFQ (NOLOCK) 
				INNER JOIN DCRM_UCDFeedback DUF (NOLOCK) ON CFQ.Id = DUF.QuestionId AND CFQ.IsActive = 1 
				INNER JOIN DCRM_CustomerCalling DCC (NOLOCK) ON DCC.ID = DUF.CustomerCallingId AND DCC.ActionID = 1 AND DCC.IsFeedbackGiven=@isFeedbackTaken
						AND CONVERT(DATE,DCC.ActionTakenOn ) = CONVERT(DATE,DUF.FBSubmitDate)
				INNER JOIN CRM_FeedbackAnswers CFA (NOLOCK) ON CFA.Id = DUF.AnswerId
				INNER JOIN UsedCarPurchaseInquiries UCP (NOLOCK) ON UCP.CustomerID = DUF.CustomerId  
					AND	 CONVERT(DATE,UCP.RequestDateTime) BETWEEN  CONVERT(DATE,@InqryFrDate) 
					AND  CONVERT(DATE,@InqryToDate)
				INNER JOIN SellInquiries SI (NOLOCK) ON SI.ID = UCP.SellInquiryId 
				INNER JOIN vwMMV VW (NOLOCK) ON VW.VersionId = SI.CarVersionId
				INNER JOIN Customers  C (NOLOCK) ON DUF.CustomerId =  C.Id
				INNER JOIN Dealers D (NOLOCK) ON D.ID = DUF.DealerId
				INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = DUF.UpdatedBy
				LEFT JOIN Cities CC (NOLOCK) ON CC.ID = C.CityId
				LEFT JOIN States S (NOLOCK) ON CC.StateId = S.ID
			WHERE   CFQ.FeedbackType = @feedbackType AND (@dealerId IS NULL OR DUF.DealerId = @dealerId) )
			SELECT * FROM ABC WHERE InquiryDate BETWEEN  @InqryFrDate AND @InqryToDate 
		END
		
	--ELSE IF @Type = 3-- feedback Not given
	--	BEGIN
	--		WITH ABC AS(
	--		SELECT  DISTINCT CFQ.Id AS QuestionId ,CFQ.Question ,CFQ.AnswerType,CFQ.Alias,CFA.Answer,DUF.DealerId,
	--			DUF.AnswerId,DUF.CustomerId,DUF.Comments,CONVERT(VARCHAR, DUF.FBSubmitDate,106) AS FBDate,'' AS RequestDateTime ,D.Organization,'' AS CarModelNames,
	--			C.Name AS CustomerName,C.ID AS CustId, OU.UserName,ISNULL(CC.Name,'') AS CityName,ISNULL(S.Name,'') AS State ,C.Mobile,DCC.InquiryDate
				
	--		FROM CRM_FeedbackQuestions CFQ (NOLOCK) 
	--			INNER JOIN DCRM_UCDFeedback DUF (NOLOCK) ON CFQ.Id = DUF.QuestionId AND CFQ.IsActive = 1 
	--			INNER JOIN DCRM_CustomerCalling DCC (NOLOCK) ON DCC.ID = DUF.CustomerCallingId AND DCC.IsFeedbackGiven=0
	--					AND CONVERT(DATE,DCC.ActionTakenOn ) = CONVERT(DATE,DUF.FBSubmitDate)
	--			INNER JOIN CRM_FeedbackAnswers CFA (NOLOCK) ON CFA.Id = DUF.AnswerId
	--			INNER JOIN UsedCarPurchaseInquiries UCP (NOLOCK) ON UCP.CustomerID = DUF.CustomerId  
	--				AND	 CONVERT(DATE,UCP.RequestDateTime) BETWEEN  CONVERT(DATE,@InqryFrDate) 
	--				AND  CONVERT(DATE,@InqryToDate)
	--			INNER JOIN SellInquiries SI (NOLOCK) ON SI.ID = UCP.SellInquiryId 
	--			INNER JOIN vwMMV VW (NOLOCK) ON VW.VersionId = SI.CarVersionId
	--			INNER JOIN Customers  C (NOLOCK) ON DUF.CustomerId =  C.Id
	--			INNER JOIN Dealers D (NOLOCK) ON D.ID = DUF.DealerId
	--			INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = DUF.UpdatedBy
	--			LEFT JOIN Cities CC (NOLOCK) ON CC.ID = C.CityId
	--			LEFT JOIN States S (NOLOCK) ON CC.StateId = S.ID
	--		WHERE   CFQ.FeedbackType = @feedbackType AND (@dealerId IS NULL OR DUF.DealerId = @dealerId))
	--		SELECT * FROM ABC WHERE InquiryDate BETWEEN  @InqryFrDate AND @InqryToDate 
	--	END
		
		ELSE IF @Type = 4 -- took the call
		BEGIN
			WITH ABC AS(
			SELECT  DISTINCT CFQ.Id AS QuestionId ,CFQ.Question ,CFQ.AnswerType,CFQ.Alias,CFA.Answer,DUF.DealerId,
				DUF.AnswerId,DUF.CustomerId,DUF.Comments,CONVERT(VARCHAR, DUF.FBSubmitDate,106) AS FBDate,'' AS RequestDateTime ,D.Organization,'' AS CarModelNames,
				C.Name AS CustomerName,C.ID AS CustId, OU.UserName,ISNULL(CC.Name,'') AS CityName,ISNULL(S.Name,'') AS State ,C.Mobile,
				DCC.InquiryDate
				
			FROM CRM_FeedbackQuestions CFQ (NOLOCK) 
				INNER JOIN DCRM_UCDFeedback DUF (NOLOCK) ON CFQ.Id = DUF.QuestionId AND CFQ.IsActive = 1 
				INNER JOIN DCRM_CustomerCalling DCC (NOLOCK) ON DCC.ID = DUF.CustomerCallingId 
						AND CONVERT(DATE,DCC.ActionTakenOn ) = CONVERT(DATE,DUF.FBSubmitDate) AND DCC.ActionID = 1 
				INNER JOIN CRM_FeedbackAnswers CFA (NOLOCK) ON CFA.Id = DUF.AnswerId
				INNER JOIN UsedCarPurchaseInquiries UCP (NOLOCK) ON UCP.CustomerID = DUF.CustomerId  
					AND	 CONVERT(DATE,UCP.RequestDateTime) BETWEEN  CONVERT(DATE,@InqryFrDate) 
					AND  CONVERT(DATE,@InqryToDate)
				INNER JOIN SellInquiries SI (NOLOCK) ON SI.ID = UCP.SellInquiryId 
				INNER JOIN vwMMV VW (NOLOCK) ON VW.VersionId = SI.CarVersionId
				INNER JOIN Customers  C (NOLOCK) ON DUF.CustomerId =  C.Id
				INNER JOIN Dealers D (NOLOCK) ON D.ID = DUF.DealerId
				INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = DUF.UpdatedBy
				LEFT JOIN Cities CC (NOLOCK) ON CC.ID = C.CityId
				LEFT JOIN States S (NOLOCK) ON CC.StateId = S.ID
			WHERE   CFQ.FeedbackType = @feedbackType AND (@dealerId IS NULL OR DUF.DealerId = @dealerId))
			SELECT * FROM ABC WHERE InquiryDate BETWEEN  @InqryFrDate AND @InqryToDate
		END
	--If user apply constraint on FeedBack taken date
	--ELSE IF @Type = 2
	--	BEGIN
	--		SELECT   DISTINCT CFQ.Id AS QuestionId ,CFQ.Question ,CFQ.AnswerType,CFQ.Alias,CFA.Answer,DUF.DealerId,
	--		DUF.AnswerId,DUF.CustomerId,DUF.Comments,CONVERT(VARCHAR, DUF.FBSubmitDate,106) AS FBDate,D.Organization,
	--		C.Name AS CustomerName,C.ID AS CustId, OU.UserName,ISNULL(CC.Name,'') AS CityName,ISNULL(S.Name,'') AS State,'' CarModelNames,
	--		'' AS RequestDateTime, CONVERT(VARCHAR, DUF.FBSubmitDate,106) FBSubmitDate, DCC.ActionTakenOn,C.Mobile
	--		FROM CRM_FeedbackQuestions CFQ (NOLOCK) 
	--		INNER JOIN DCRM_UCDFeedback DUF (NOLOCK) ON CFQ.Id = DUF.QuestionId AND CFQ.IsActive = 1 
	--		INNER JOIN DCRM_CustomerCalling DCC (NOLOCK) ON DCC.CustomerId = DUF.CustomerId
	--			AND CONVERT(DATE,DCC.ActionTakenOn ) = CONVERT(DATE,DUF.FBSubmitDate)
	--		INNER JOIN UsedCarPurchaseInquiries UCP (NOLOCK) ON UCP.CustomerID = DCC.CustomerId  
	--		INNER JOIN SellInquiries SI (NOLOCK) ON SI.ID = UCP.SellInquiryId 
	--		INNER JOIN vwMMV VW (NOLOCK) ON VW.VersionId = SI.CarVersionId
	--		INNER JOIN CRM_FeedbackAnswers CFA (NOLOCK) ON CFA.Id = DUF.AnswerId
	--		INNER JOIN Customers  C (NOLOCK) ON DUF.CustomerId =  C.Id
	--		INNER JOIN Dealers D (NOLOCK) ON D.ID = DUF.DealerId
	--		INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = DUF.UpdatedBy
	--		LEFT JOIN Cities CC (NOLOCK) ON CC.ID = C.CityId
	--		LEFT JOIN States S (NOLOCK) ON CC.StateId = S.ID
	--		WHERE   CFQ.FeedbackType = @feedbackType AND (@dealerId IS NULL OR DUF.DealerId = @dealerId)
	--			AND  (CONVERT(DATE,DUF.FBSubmitDate) BETWEEN  CONVERT(DATE,@frmDate) AND  CONVERT(DATE,@toDate)) 
	--	END
END