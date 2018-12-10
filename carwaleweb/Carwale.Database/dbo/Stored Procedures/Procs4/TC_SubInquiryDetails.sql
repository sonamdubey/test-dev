IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SubInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SubInquiryDetails]
GO

	
-- =============================================
-- Modified By:		<Nilesh Utture>
-- Modified date:   <18/02/2013>
-- Description:		Added Condition IF(@TC_LeadDispositionId = 4)
-- Modified By Surendra On 27-02-2013 Changes- TCIL.CreatedDate -  TL.LeadCreationDate BETWEEN @FromDate AND @ToDate
-- =============================================
CREATE PROCEDURE [dbo].[TC_SubInquiryDetails]
  (
	@BranchId int,
	@FromDate DATETIME,
	@ToDate  DATETIME,
	@IsFollowUP BIT =0,
	@TC_LeadInquiryType TINYINT=NULL,
	@TC_LeadDispositionId TINYINT=NULL,
	@TC_LeadStageId  TINYINT=NULL
   )
AS
BEGIN
	IF @IsFollowUP=0
	 
		IF(@TC_LeadDispositionId = 4)
			BEGIN
				WITH CTE  AS 
								 ( SELECT  CarDetails,TC_LeadId,TC_LeadDispositionID,CreatedDate,TC_UserId,ROW_NUMBER() OVER(PARTITION BY TC_LeadId ORDER BY LatestInquiryDate DESC) AS ROWNUM
									FROM TC_InquiriesLead  WITH(NOLOCK)
									WHERE BranchId	= @BranchId             
										AND  TC_LeadInquiryTypeId=ISNULL(@TC_LeadInquiryType,TC_LeadInquiryTypeId)
										AND  TC_LeadDispositionId=4 
										AND  TC_LeadInquiryTypeId IN (1,2,3)
										)
						SELECT DISTINCT 
							   TC.Id , 
							   TC.CustomerName, 
							   TC.Email, 
							   TC.Mobile, 
							   TC.Location, 
							   TL.LeadCreationDate AS EntryDate, 
							   TU.UserName,
							  TCIL.CarDetails
							FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
							 JOIN  TC_Lead          AS TL   WITH(NOLOCK)ON TL.TC_CustomerId= TC.Id 
							 JOIN  CTE              AS TCIL             ON TL.TC_LeadId=TCIL.TC_LeadId AND ROWNUM=1
												  AND   TL.BranchId	= @BranchId 
												  AND  TCIL.TC_LeadDispositionId=4
												  AND  TL.LeadCreationDate BETWEEN @FromDate AND @ToDate
							 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
													  
							ORDER BY EntryDate DESC
			END
		ELSE
			BEGIN
				  WITH CTE  AS 
							 ( SELECT  CarDetails,TC_LeadId,TC_UserId,ROW_NUMBER() OVER(PARTITION BY TC_LeadId ORDER BY LatestInquiryDate DESC) AS ROWNUM
								FROM TC_InquiriesLead  WITH(NOLOCK)
								WHERE BranchId	= @BranchId             
									AND  TC_LeadInquiryTypeId=ISNULL(@TC_LeadInquiryType,TC_LeadInquiryTypeId) 
									AND  TC_LeadInquiryTypeId IN (1,2,3)
									)
					SELECT DISTINCT 
						   TC.Id , 
						   TC.CustomerName, 
						   TC.Email, 
						   TC.Mobile, 
						   TC.Location, 
						   TL.LeadCreationDate AS EntryDate, 
						   TU.UserName,
						  TCIL.CarDetails
						FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
						 JOIN  TC_Lead          AS TL   WITH(NOLOCK)ON TL.TC_CustomerId= TC.Id 
						 JOIN  CTE              AS TCIL             ON TL.TC_LeadId=TCIL.TC_LeadId AND ROWNUM=1
											  AND   TL.BranchId	= @BranchId 
											  AND  ISNULL(TL.TC_LeadDispositionId,1)=ISNULL(ISNULL(@TC_LeadDispositionId,TL.TC_LeadDispositionId),1)
											  AND  ISNULL(TL.TC_LeadStageId,1)=ISNULL(ISNULL(@TC_LeadStageId,TL.TC_LeadStageId),1)
											  AND  TL.LeadCreationDate BETWEEN @FromDate AND @ToDate
						 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
												  
						ORDER BY EntryDate DESC
			END
			
	ELSE
		WITH CTE  AS 
					 ( SELECT  CarDetails,TC_LeadId,TC_UserId,ROW_NUMBER() OVER(PARTITION BY TC_LeadId ORDER BY LatestInquiryDate DESC) AS ROWNUM
						FROM TC_InquiriesLead  WITH(NOLOCK)
						WHERE BranchId	= @BranchId             
							AND  TC_LeadInquiryTypeId=ISNULL(@TC_LeadInquiryType,TC_LeadInquiryTypeId) 
							AND  TC_LeadInquiryTypeId IN (1,2,3)
							)
			SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, 
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   TL.LeadCreationDate AS EntryDate, 
				   TU.UserName,
				  TCIL.CarDetails
	   			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
	   			 JOIN  TC_Lead          AS TL   WITH(NOLOCK)ON TL.TC_CustomerId= TC.Id 
	   			 JOIN  CTE              AS TCIL             ON TL.TC_LeadId=TCIL.TC_LeadId AND ROWNUM=1
									  AND   TL.BranchId	= @BranchId 
									  --AND  TL.TC_LeadDispositionId=ISNULL(@TC_LeadDispositionId,TL.TC_LeadDispositionId)
									  AND  (TL.TC_LeadStageId=1 OR TL.TC_LeadStageId=2)
									  AND  TL.LeadCreationDate BETWEEN @FromDate AND @ToDate
				 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
										  
				ORDER BY EntryDate DESC
END



