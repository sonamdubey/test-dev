IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_changeInquiryDisposition]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_changeInquiryDisposition]
GO

	
-- =============================================  
-- Author:  <Author,Nilesh Utture>  
-- Create date: <Create Date,16/01/2013>  
-- Description: <Description, This SP will give list of all inquiries for a particular customer>  
-- Modified By : Nilesh Utture on 14th February 2013, commented --OR TC_LeadDispositionID = 4) condition
-- EXEC TC_changeInquiryDisposition 132,8,1
-- Modified By: Nilesh Utture on 19th Feb, 2013 Added  "IsActive = 1 in WHERE CLAUSE OF TC_InquiriesLead TABLE"
-- Modified By: Nilesh Utture on 8th April, 2013 Changed the execution process of EXEC TC_DispositionLogInsert stored procedure 
--				So that Ones the inquiry is closed only one disposition will be entered into log table
-- Modified by: Nilesh Utture on 24th April,2013 added parameter @TC_UserId to SP 
-- Modified by: Nilesh Utture on 21st June, 2013 Added Condition (OR TC_LeadDispositionId = 4)
-- Modified By: Tejashree Patil on 15 Jul 2013, Added parameter @ActionTakenOn instead of GETDATE() for import excel and commented GETDATE()
-- Modified by: Nilesh Utture on 26th July, 2013 Added Condition to NewCarInquiry Type "AND ISNULL(TCNI.CarDeliveryStatus, 0) <> 77"
--				This condition denotes that the particular inquiry is closed
-- Modified By : Tejashree Patil on 17 Juy 2013 , Added @SubDisposition parameter and processed TC_LeadSubDispositions
-- Modified By Vivek Gupta on 18-07-2014, Added with nolock in select queries
-- Modified By Khushaboo Patil 17-07-2015 added parameters @DispositionReason,@CityId
-- Modified by Manish commented print statement and changed datatype from bigint to int.
-- Modified By Vivek Gupta on 12th jan 2016, inserted leadsubdispositions in disposition log table
-- Modified By Chetan Navin on 21st June 2016.
-- Modified By : Nilima More On 4th June 2016,added lead closed condition for service inquiries,when Action like booking cancelled,markas delivered and reopen
--exec TC_changeInquiryDisposition 11,105,4,88916,null,null,null,null
-- Modified by : Ruchira Patil 8th Aug 2016 (updated column DispositionComment in tc_service_inquiries)
-- Modified by : Tejashree Patil on 30 August 2016, added logic for inquiryType = 6 ie. insurance inquiry.
-- =============================================  
CREATE PROCEDURE  [dbo].[TC_changeInquiryDisposition]
	-- Add the parameters for the stored procedure here
	 @InquiryId INT,  
	 @Disposition INT,  
	 @InquiryType TINYINT,
	 @TC_UserId INT = NULL ,	  -- UserId of user who is trying to modify/ close inquiry
	 @ActionTakenOn DATETIME = NULL ,-- Modified By: Tejashree Patil on 10 Jul 2013
	 @SubDisposition INT = NULL,
	 @LostVersionId INT = NULL,
	 @DispositionReason	VARCHAR(200) = NULL,
	 @CityId	INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	
	SET NOCOUNT ON;
	DECLARE @LeadId INT
	DECLARE @UserId INT 
	DECLARE @InquiriesLeadId INT
	DECLARE @BuyerCount INT
	DECLARE @SellerCount INT
	DECLARE @NewCarCount INT
	DECLARE @InquiriesLeadCount INT
	DECLARE @CarDetailUpdateRequired BIT=0
	DECLARE @ActiveCallDeleteRequired BIT=0
	DECLARE @LeadOwnerId INT -- Lead owner Id -- Modified by: Nilesh Utture on 24th April,2013
	--DECLARE @UsersOtherInqLeadExists BIT=0
	DECLARE @ServiceCount INT --Added By : Nilima More On 1st july 2016.
	
	SET		@ActionTakenOn = ISNULL(@ActionTakenOn,GETDATE())-- Modified By: Tejashree Patil on 15 Jul 2013
	
	--SET @UserId = @TC_UserId

    IF @InquiryType = 1 -- change disposition for Buyer Inquiry 
		BEGIN
			SELECT @InquiriesLeadId =  TC_InquiriesLeadId FROM TC_BuyerInquiries WITH (NOLOCK) WHERE TC_BuyerInquiriesId = @InquiryId
			SELECT @LeadId = TC_LeadId, @LeadOwnerId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId AND IsActive = 1
			IF @TC_UserId IS NULL -- If userId is NULL @UserId will be leadOwnerId -- Modified by: Nilesh Utture on 24th April,2013
			BEGIN
			SELECT @UserId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId AND IsActive = 1
			END
			ELSE -- UserId
			BEGIN
			SELECT @UserId = @TC_UserId
			END
			
			UPDATE TC_BuyerInquiries SET TC_LeadDispositionId = @Disposition WHERE TC_BuyerInquiriesId = @InquiryId
			SELECT @BuyerCount = COUNT(TC_BuyerInquiriesId) FROM TC_BuyerInquiries WITH (NOLOCK) WHERE TC_LeadDispositionId IS NULL AND TC_InquiriesLeadId = @InquiriesLeadId 
			
			IF @BuyerCount = 0 -- if buyer inquiries are not prestent then dispose buyer lead
				BEGIN
					IF EXISTS(SELECT TC_InquiriesLeadId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId AND TC_LeadDispositionId IS NULL  AND IsActive = 1)
						BEGIN
							UPDATE TC_InquiriesLead SET TC_LeadStageId = 3, TC_LeadDispositionId = @Disposition WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1
						END
					ELSE
						BEGIN
							UPDATE TC_InquiriesLead SET TC_LeadStageId = 3 WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1
						END
				END
			ELSE
				BEGIN
					EXEC TC_DispositionLogInsert @UserId,@Disposition,@InquiryId,3,@LeadId,NULL,@DispositionReason,@SubDisposition -- Modified By: Nilesh Utture on 8th April, 2013
					SET @CarDetailUpdateRequired=1
				END
				
			--SELECT @InquiriesLeadCount = COUNT(TC_InquiriesLeadId) FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_LeadId = @LeadId AND TC_LeadDispositionId IS NULL  AND IsActive = 1 --OR TC_LeadDispositionID = 4) 
			SELECT @InquiriesLeadCount = COUNT(IL.TC_InquiriesLeadId) FROM  TC_InquiriesLead	 AS IL	 WITH(NOLOCK) 
																	   LEFT JOIN  TC_NewCarInquiries AS TCNI WITH (NOLOCK) ON       IL.TC_InquiriesLeadId = TCNI.TC_InquiriesLeadId 
																	   LEFT JOIN  TC_SellerInquiries AS TCSI WITH (NOLOCK) ON	   IL.TC_InquiriesLeadId = TCSI.TC_InquiriesLeadId
																	   LEFT JOIN  TC_BuyerInquiries  AS TCBI WITH (NOLOCK) ON	   IL.TC_InquiriesLeadId = TCBI.TC_InquiriesLeadId																	  
																	  WHERE IL.TC_LeadId = @LeadId AND (TCNI.TC_LeadDispositionID IS NULL OR (TCNI.TC_LeadDispositionID = 4 AND ISNULL(TCNI.CarDeliveryStatus, 0) <> 77)) AND IL.IsActive = 1
																								AND  TCBI.TC_LeadDispositionId IS NULL 
																								AND  TCSI.TC_LeadDispositionID IS NULL
			
			IF @InquiriesLeadCount = 0 AND @BuyerCount = 0 -- If sell, buy, new Inquiries leads are not present then dispose tc_Lead
				BEGIN
					UPDATE TC_Lead SET TC_LeadStageId = 3,LeadClosedDate = GETDATE() WHERE TC_LeadId = @LeadId
					UPDATE TC_CustomerDetails SET ActiveLeadId = NULL, IsleadActive = 0--, IsActive = 0 
					WHERE ActiveLeadId = @LeadId
					EXEC TC_DispositionLogInsert @UserId,@Disposition,@LeadId,1,@LeadId,NULL,@DispositionReason,@SubDisposition
					SET @ActiveCallDeleteRequired=1 -- Active Call deletion required
				END
			ELSE IF @InquiriesLeadCount <> 0 AND @BuyerCount = 0 -- Modified By: Nilesh Utture on 8th April, 2013
				BEGIN
					EXEC TC_DispositionLogInsert @UserId,@Disposition,@InquiriesLeadId,2,@LeadId,NULL,@DispositionReason,@SubDisposition
				END
		END	 
		
	IF @InquiryType = 2 -- change disposition for seller Inquiry 
		BEGIN
			SELECT @InquiriesLeadId =  TC_InquiriesLeadId FROM TC_SellerInquiries WITH (NOLOCK) WHERE TC_SellerInquiriesId = @InquiryId
			SELECT @LeadId = TC_LeadId, @LeadOwnerId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1
			IF @TC_UserId IS NULL -- Modified by: Nilesh Utture on 24th April,2013
			BEGIN
			SELECT @UserId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId AND IsActive = 1
			END
			ELSE
			BEGIN
			SELECT @UserId = @TC_UserId
			END
			
			IF @Disposition = 33 -- In case of seller Inquiry update status of inquiry to purchased
				BEGIN
					UPDATE TC_SellerInquiries SET PurchasedStatus = 33, PurchasedDate=GETDATE() WHERE TC_SellerInquiriesId = @InquiryId
					EXEC TC_DispositionLogInsert @UserId,33,@InquiryId,4,@LeadId,NULL,@DispositionReason,@SubDisposition
				END
			ELSE -- to close the inquiry of seller type
				BEGIN
					UPDATE TC_SellerInquiries SET TC_LeadDispositionId = @Disposition WHERE TC_SellerInquiriesId = @InquiryId
					SELECT @SellerCount = COUNT(TC_SellerInquiriesId) FROM TC_SellerInquiries WITH (NOLOCK) WHERE TC_LeadDispositionId IS NULL AND TC_InquiriesLeadId = @InquiriesLeadId

					IF @SellerCount = 0 -- if seller inquiries are not prestent then dispose seller lead
						BEGIN
							IF EXISTS(SELECT TC_InquiriesLeadId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId AND TC_LeadDispositionId IS NULL  AND IsActive = 1)
								BEGIN
									UPDATE TC_InquiriesLead SET TC_LeadStageId = 3, TC_LeadDispositionId = @Disposition WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1
								END
							ELSE
								BEGIN
									UPDATE TC_InquiriesLead SET TC_LeadStageId = 3 WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1
								END
						END
					ELSE
						BEGIN
							EXEC TC_DispositionLogInsert @UserId,@Disposition,@InquiryId,4,@LeadId,NULL,@DispositionReason,@SubDisposition -- Modified By: Nilesh Utture on 8th April, 2013
							SET @CarDetailUpdateRequired=1
						END
					
					--SELECT @InquiriesLeadCount = COUNT(TC_InquiriesLeadId) FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_LeadId = @LeadId AND TC_LeadDispositionId IS NULL  AND IsActive = 1 --OR TC_LeadDispositionID = 4) 
					SELECT @InquiriesLeadCount = COUNT(IL.TC_InquiriesLeadId) FROM  TC_InquiriesLead	 AS IL	 WITH(NOLOCK) 
																	   LEFT JOIN  TC_NewCarInquiries AS TCNI WITH (NOLOCK) ON       IL.TC_InquiriesLeadId = TCNI.TC_InquiriesLeadId 
																	   LEFT JOIN  TC_SellerInquiries AS TCSI WITH (NOLOCK) ON	   IL.TC_InquiriesLeadId = TCSI.TC_InquiriesLeadId
																	   LEFT JOIN  TC_BuyerInquiries  AS TCBI WITH (NOLOCK) ON	   IL.TC_InquiriesLeadId = TCBI.TC_InquiriesLeadId																	  
																	  WHERE IL.TC_LeadId = @LeadId AND (TCNI.TC_LeadDispositionID IS NULL OR (TCNI.TC_LeadDispositionID = 4 AND ISNULL(TCNI.CarDeliveryStatus, 0) <> 77)) AND IL.IsActive = 1
																								AND  TCBI.TC_LeadDispositionId IS NULL 
																								AND  TCSI.TC_LeadDispositionID IS NULL
					
					IF @InquiriesLeadCount = 0 AND @SellerCount = 0 -- If sell, buy, new Inquiries leads are not present then dispose tc_Lead
						BEGIN
							UPDATE TC_Lead SET TC_LeadStageId = 3,LeadClosedDate = GETDATE() WHERE TC_LeadId = @LeadId
							UPDATE TC_CustomerDetails SET ActiveLeadId = NULL, IsleadActive = 0--, IsActive = 0 
							WHERE ActiveLeadId = @LeadId							
							EXEC TC_DispositionLogInsert @UserId,@Disposition,@LeadId,1,@LeadId,NULL,@DispositionReason,@SubDisposition
							SET @ActiveCallDeleteRequired=1 -- Active Call deletion required							
						END
					ELSE IF @InquiriesLeadCount <> 0 AND @SellerCount = 0 -- Modified By: Nilesh Utture on 8th April, 2013
						BEGIN
							EXEC TC_DispositionLogInsert @UserId,@Disposition,@InquiriesLeadId,2,@LeadId,NULL,@DispositionReason,@SubDisposition
						END
				END
		END	
		
	IF @InquiryType = 3 -- change disposition for New car Inquiry 
		BEGIN
			SELECT @InquiriesLeadId =  TC_InquiriesLeadId FROM TC_NewCarInquiries WITH (NOLOCK) WHERE TC_NewCarInquiriesId = @InquiryId
			SELECT @LeadId = TC_LeadId, @LeadOwnerId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1
			IF @TC_UserId IS NULL -- Modified by: Nilesh Utture on 24th April,2013
			BEGIN
			SELECT @UserId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId AND IsActive = 1
			END
			ELSE
			BEGIN
			SELECT @UserId = @TC_UserId
			END
			
			UPDATE	TC_NewCarInquiries 
			SET		TC_LeadDispositionId = @Disposition, 
					TC_SubDispositionId =  @SubDisposition, --Tejashree Patil on 17 July 2013
					DispositionDate  =@ActionTakenOn,
					LostVersionId = @LostVersionId,
					TC_LeadDispositionReason = @DispositionReason,
					CityId = CASE WHEN @CityId IS NULL THEN CityId ELSE @CityId END
			WHERE	TC_NewCarInquiriesId = @InquiryId
			
			SELECT @NewCarCount = COUNT(TC_NewCarInquiriesId) FROM TC_NewCarInquiries WITH (NOLOCK) 
			WHERE (TC_LeadDispositionId IS NULL OR (TC_LeadDispositionId = 4 AND ISNULL(CarDeliveryStatus, 0) <> 77) )  
			AND TC_InquiriesLeadId = @InquiriesLeadId -- Modified by: Nilesh Utture on 21st June, 2013
																																																															   -- Modified by: Nilesh Utture on 26st July, 2013

			IF @NewCarCount = 0 -- if new car inquiries are not prestent then dispose new car lead
				BEGIN
					IF EXISTS(SELECT TC_InquiriesLeadId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId AND TC_LeadDispositionId IS NULL  AND IsActive = 1)
						BEGIN
							UPDATE TC_InquiriesLead SET TC_LeadStageId = 3, TC_LeadDispositionId = @Disposition WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1
						END
					ELSE
						BEGIN
							UPDATE TC_InquiriesLead SET TC_LeadStageId = 3 WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1
						END
				END
			ELSE
				BEGIN
					EXEC TC_DispositionLogInsert @UserId,@Disposition,@InquiryId,5,@LeadId,NULL,@DispositionReason,@SubDisposition -- Modified By: Nilesh Utture on 8th April, 2013
					--Tejashree Patil on 17 July 2013
					SET @CarDetailUpdateRequired=1
				END
				
			--SELECT @InquiriesLeadCount = COUNT(TC_InquiriesLeadId) FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_LeadId = @LeadId AND (TC_LeadDispositionId IS NULL OR TC_LeadDispositionID = 4) AND IsActive = 1 --OR TC_LeadDispositionID = 4)-- Modified by: Nilesh Utture on 21st June, 2013
			SELECT @InquiriesLeadCount = COUNT(IL.TC_InquiriesLeadId) FROM  TC_InquiriesLead	 AS IL	 WITH(NOLOCK) 
																	  JOIN  TC_NewCarInquiries AS TCNI WITH (NOLOCK) 
																	  ON    IL.TC_InquiriesLeadId = TCNI.TC_InquiriesLeadId 
																	  WHERE IL.TC_LeadId = @LeadId AND (TCNI.TC_LeadDispositionID IS NULL OR (TCNI.TC_LeadDispositionID = 4 AND ISNULL(TCNI.CarDeliveryStatus, 0) <> 77)) AND IL.IsActive = 1 -- Modified by: Nilesh Utture on 26st July, 2013
			--PRINT @InquiriesLeadCount
			IF @InquiriesLeadCount = 0 AND @NewCarCount = 0 -- If sell, buy, new Inquiries leads are not present then dispose tc_Lead
				BEGIN
					UPDATE TC_Lead SET TC_LeadStageId = 3, LeadClosedDate = GETDATE() WHERE TC_LeadId = @LeadId
					UPDATE TC_CustomerDetails SET ActiveLeadId = NULL, IsleadActive = 0 --,IsActive = 0 
					WHERE ActiveLeadId = @LeadId
					EXEC TC_DispositionLogInsert @UserId,@Disposition,@LeadId,1,@LeadId,NULL,@DispositionReason,@SubDisposition 
					SET @ActiveCallDeleteRequired=1 -- Active Call deletion required
				END
			ELSE IF @InquiriesLeadCount <> 0 AND @NewCarCount = 0 -- Modified By: Nilesh Utture on 8th April, 2013
				BEGIN
					EXEC TC_DispositionLogInsert @UserId,@Disposition,@InquiriesLeadId,2,@LeadId,NULL,@DispositionReason,@SubDisposition 
				END
		END	
	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	--Added by : Nilima More,Handle lead disposition on every action.
	IF @InquiryType = 4 --Service Inquiries
	BEGIN
			SELECT @InquiriesLeadId =  TC_InquiriesLeadId FROM TC_Service_Inquiries WITH (NOLOCK) WHERE TC_Service_InquiriesId = @InquiryId

			SELECT @LeadId = TC_LeadId, @LeadOwnerId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1

			IF @TC_UserId IS NULL 
			BEGIN
			SELECT @UserId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId AND IsActive = 1
			END
			ELSE
			BEGIN
			SELECT @UserId = @TC_UserId
			END
			
			--update TC_LeadDispositionId in serviceInquiries as per action.
			UPDATE TC_Service_Inquiries 
			SET TC_LeadDispositionId = @Disposition,DispositionComment = @DispositionReason -- Modified by : Ruchira Patil 8th Aug 2016 (updated column DispositionComment in tc_service_inquiries
			WHERE TC_Service_InquiriesId  =@InquiryId
		
			UPDATE TC_InquiriesLead SET TC_LeadStageId = 3, TC_LeadDispositionId = @Disposition WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1
						
			--disposition item 7 = service inq
			EXEC TC_DispositionLogInsert @UserId,@Disposition,@InquiryId,7,@LeadId,NULL,@DispositionReason,@SubDisposition 
			SET @CarDetailUpdateRequired=1
				
			UPDATE TC_Lead SET TC_LeadStageId = 3, LeadClosedDate = GETDATE(),TC_LeadDispositionId = @Disposition WHERE TC_LeadId = @LeadId
			UPDATE TC_CustomerDetails SET ActiveLeadId = NULL, IsleadActive = 0 
			WHERE ActiveLeadId = @LeadId

			SET @ActiveCallDeleteRequired=1 -- Active Call deletion required
	END
	-- Modified by : Tejashree Patil on 30 August 2016, added logic for inquiryType = 6 ie. insurance inquiry.
	IF @InquiryType = 6--Insurance Inquiries
	BEGIN
			SELECT @InquiriesLeadId =  TC_InquiriesLeadId FROM TC_Insurance_Inquiries WITH (NOLOCK) WHERE TC_Insurance_InquiriesId = @InquiryId

			SELECT @LeadId = TC_LeadId, @LeadOwnerId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1

			IF @TC_UserId IS NULL 
			BEGIN
			SELECT @UserId = TC_UserId FROM TC_InquiriesLead WITH (NOLOCK) WHERE TC_InquiriesLeadId = @InquiriesLeadId AND IsActive = 1
			END
			ELSE
			BEGIN
			SELECT @UserId = @TC_UserId
			END
			
			--update TC_LeadDispositionId in TC_Insurance_Inquiries as per action.
			UPDATE TC_Insurance_Inquiries 
			SET TC_LeadDispositionId = @Disposition  -- Modified by : Ruchira Patil 8th Aug 2016 (updated column DispositionComment in tc_service_inquiries
			WHERE TC_Insurance_InquiriesId  =@InquiryId
		
			UPDATE TC_InquiriesLead SET TC_LeadStageId = 3, TC_LeadDispositionId = @Disposition WHERE TC_InquiriesLeadId = @InquiriesLeadId  AND IsActive = 1
						
			--disposition item 7 = service inq
			EXEC TC_DispositionLogInsert @UserId,@Disposition,@InquiryId,7,@LeadId,NULL,@DispositionReason,@SubDisposition 
			SET @CarDetailUpdateRequired=1
				
			UPDATE TC_Lead SET TC_LeadStageId = 3, LeadClosedDate = GETDATE(),TC_LeadDispositionId = @Disposition WHERE TC_LeadId = @LeadId
			UPDATE TC_CustomerDetails SET ActiveLeadId = NULL, IsleadActive = 0 
			WHERE ActiveLeadId = @LeadId

			SET @ActiveCallDeleteRequired=1 -- Active Call deletion required
	END
	-------------------------------------------------------------------------------------------------------------------------------------------------------------

	--Since current inquiry is dispose hence latest car details need to update in tc_inquiries lead table
	IF(@CarDetailUpdateRequired=1)
	BEGIN
		DECLARE @CarDetails VARCHAR(200)
		IF(@InquiryType=1)
		BEGIN
			--DECLARE @StockId BIGINT =NULL
			SELECT TOP 1 @CarDetails=M.Car FROM TC_BuyerInquiries B WITH (NOLOCK) 
				LEFT JOIN TC_Stock S WITH (NOLOCK) ON B.StockId=S.Id
				LEFT JOIN vwMMV M WITH (NOLOCK) ON S.VersionId=M.VersionId
			WHERE TC_InquiriesLeadId=@InquiriesLeadId AND TC_LeadDispositionId IS NULL
			ORDER BY CreatedOn DESC
			
			IF(@CarDetails IS NULL)
			BEGIN
				SELECT TOP 1 @CarDetails=ISNULL(CAST(B.MakeYearFrom AS VARCHAR),'') + ' ' + ISNULL(CAST(B.MakeYearTo AS VARCHAR),'') 
										+ ' ' +ISNULL(CAST(B.PriceMin AS VARCHAR),'') + ' ' +ISNULL(CAST(B.PriceMax AS VARCHAR),'')
				FROM TC_BuyerInquiries B WITH (NOLOCK)
				WHERE TC_InquiriesLeadId=@InquiriesLeadId AND TC_LeadDispositionId IS NULL
				ORDER BY CreatedOn DESC
			END
		END
		
		IF(@InquiryType=2)
		BEGIN
			SELECT TOP 1 @CarDetails=M.Car FROM TC_SellerInquiries S WITH (NOLOCK) INNER JOIN vwMMV M WITH (NOLOCK) ON S.CarVersionId=M.VersionId
			WHERE TC_InquiriesLeadId=@InquiriesLeadId AND TC_LeadDispositionId IS NULL
			ORDER BY CreatedOn DESC
		END
		
		IF(@InquiryType=3)
		BEGIN
			SELECT TOP 1 @CarDetails=M.Car FROM TC_NewCarInquiries N WITH (NOLOCK) INNER JOIN vwMMV M WITH (NOLOCK) ON N.VersionId=M.VersionId
			WHERE TC_InquiriesLeadId=@InquiriesLeadId AND (TC_LeadDispositionId IS NULL OR TC_LeadDispositionId = 4)-- Modified by: Nilesh Utture on 21st June, 2013
			ORDER BY CreatedOn DESC
		END
		
		-------------Added by : Nilima More On 1st July 2016---------------
		IF(@InquiryType = 4)
		BEGIN
			SELECT TOP 1 @CarDetails=M.Car FROM TC_Service_Inquiries N WITH (NOLOCK) INNER JOIN vwMMV M WITH (NOLOCK) ON N.VersionId=M.VersionId
			WHERE TC_InquiriesLeadId=@InquiriesLeadId AND (TC_LeadDispositionId IS NULL OR TC_LeadDispositionId = 4)
			ORDER BY EntryDate DESC
		END

		UPDATE	TC_InquiriesLead SET CarDetails=@CarDetails WHERE TC_InquiriesLeadId=@InquiriesLeadId  AND IsActive = 1		
	END
	
	
	-- deleting active call on lead level
	DECLARE @TC_CallsId INT
	IF(@ActiveCallDeleteRequired=1) -- Active Call deletion required
	BEGIN		
		SELECT @TC_CallsId = TC_CallsId 
		FROM   TC_ActiveCalls WITH (NOLOCK)
		WHERE  TC_UsersId = @LeadOwnerId -- Active call of lead owner should be deleted -- Modified by: Nilesh Utture on 24th April,2013
		AND TC_LeadId = @LeadId 
             
			 
		EXEC TC_DisposeCall	@TC_CallsId,NULL,NULL,NULL,@UserId  
	END
	
	-- Deleting active call for same user if all inq leads for same user is closed
	DECLARE @ActiveInqLeadCount TINYINT
	SELECT @ActiveInqLeadCount=COUNT(*) FROM TC_InquiriesLead L WITH (NOLOCK) WHERE TC_UserId=@LeadOwnerId AND TC_LeadId=@LeadId AND TC_LeadStageId=2  AND L.IsActive = 1
	IF(@ActiveInqLeadCount=0 AND @ActiveCallDeleteRequired<>1)
	BEGIN			
			SELECT @TC_CallsId = TC_CallsId 
			FROM   TC_ActiveCalls WITH (NOLOCK)
			WHERE  TC_UsersId = @LeadOwnerId  -- Modified by: Nilesh Utture on 24th April,2013
			AND TC_LeadId = @LeadId 
			
			EXEC TC_DisposeCall	@TC_CallsId,NULL,NULL,NULL,@UserId  -- Needs review	
	END	


	
END
-----------------------------------
