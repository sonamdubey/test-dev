IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMNSCApprovalButton]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMNSCApprovalButton]
GO

	-- =================================================================
-- Author	    :	Vinayk Patil
-- Create date	:	28-11-2013
-- Description	:	SP for Approval Button on NSC screen 
-- Modified By	:	Nilesh Utture on 13-12-13, Added @Year parameter while updating approval master table 
-- Modified By  :   Nilesh Utture on 17th Dec, 2013 Added @Comments parameter
-- Modified By  :   Manish on 11-03-2014 for inserting records other than retail also.
-- ==================================================================

CREATE PROCEDURE [dbo].[TC_TMNSCApprovalButton]
@AprroversId INT,		------ ID of RM who approves or rejects revised targets
@TC_AMId INT,	------ ID of AM for whose targets are approved or rejected
@IsApproved BIT,    ------ 1 for approval & 0 for rejection
@Year INT,
@Comments VARCHAR(1000) = NULL
AS 
 
 BEGIN
			DECLARE @InquiryTarget FLOAT = 10,
					@BookingTarget FLOAT = 1.10,
					@TDTarget FLOAT = 10 * 0.75

	IF( @IsApproved = 1 )
		BEGIN
			---------- If NSC approves the target for particular AM then delete previous records for all the -----------
			----------  dealers belonging to that Area Manager & then insert updated records for the same --------------
			--DELETE DT
			--FROM   TC_DealersTarget DT WITH (NOLOCK)
			--	   INNER JOIN TC_TMAMTargetChangeApprovalReq TCR WITH (NOLOCK)
			--			   ON  DT.TC_DealersTargetId = TCR.TC_DealersTargetId 
			--WHERE  TCR.TC_AMId = @TC_AMId AND TCR.Year = @Year
				
			DELETE DT
			FROM   TC_DealersTarget DT WITH (NOLOCK)
				   INNER JOIN TC_TMAMTargetChangeApprovalReq TCR WITH (NOLOCK)
						   ON  DT.DealerId = TCR.DealerId
						   AND DT.[Month] = TCR.[Month]
						   AND DT.[Year] = TCR.[Year]
						   AND DT.CarVersionId = TCR.CarVersionId
			WHERE  TCR.TC_AMId = @TC_AMId
				  


			--------------- TO Enter Retail target data-----------------------------
 
			INSERT INTO TC_DealersTarget
						(DealerId,
						 Month,
						 Year,
						 Target,
						 CreatedBy,
						 IsDeleted,
						 TC_TargetTypeId,
						 CarVersionId)
			SELECT DealerId,
				   Month,
				   Year,
				   Target,
				   CreatedBy,
				   IsDeleted,
				   TC_TargetTypeId,
				   CarVersionId
			FROM   TC_TMAMTargetChangeApprovalReq WITH (NOLOCK)
			WHERE  TC_AMId = @TC_AMId AND Year = @Year

				UNION ALL
			----------------- TO Enter Inquiry target data-----------------------------
 				SELECT DealerId,
					   Month,
					   Year,
					   ROUND(Target * @InquiryTarget*1.0000,0),
					   CreatedBy,
					   IsDeleted,
					   1,
					   CarVersionId
				FROM   TC_TMAMTargetChangeApprovalReq WITH (NOLOCK)
				WHERE  TC_AMId = @TC_AMId AND Year = @Year

				UNION ALL
				----------------- TO Enter Test drive target data-----------------------------
 				SELECT DealerId,
					   Month,
					   Year,
					  ROUND(Target * @TDTarget*1.0000,0),
					   CreatedBy,
					   IsDeleted,
					   2,
					   CarVersionId
				FROM   TC_TMAMTargetChangeApprovalReq WITH (NOLOCK)
				WHERE  TC_AMId = @TC_AMId AND Year = @Year 

				UNION ALL
				--------------- TO Enter Inquiry Bookings data-----------------------------
 				SELECT DealerId,
					   Month,
					   Year,
					   ROUND(Target * @BookingTarget*1.0000,0),
					   CreatedBy,
					   IsDeleted,
					   3,
					   CarVersionId
				FROM   TC_TMAMTargetChangeApprovalReq WITH (NOLOCK)
				WHERE  TC_AMId = @TC_AMId AND Year = @Year 
	 

			
	 
			---------- DELETE DATA FROM TC_TMAMTargetChangeApprovalReq AFTER TARGETS ARE COPIED INTO TC_DealersTarget TABLE

			DELETE FROM TC_TMAMTargetChangeApprovalReq
			WHERE TC_AMId = @TC_AMId AND Year = @Year
			
			
			----------  Also update TC_TMAMTargetChangeMaster for that AM, make IsActive bit 0  ----------------------------------
			----------  to indicate that, that record is no longer active --------------------------------------------------------	
			
			
			  UPDATE TC_TMAMTargetChangeMaster
			  SET    IsAprrovedByNSC = 1,
					 NSCActionDate = GETDATE(),
					 NSCId = @AprroversId,
					 NSCComments = @Comments, -- Modified By  :   Nilesh Utture on 17th Dec, 2013
					 IsActive = 0
			  WHERE  TC_AMId = @TC_AMId
					 AND [Year] = @Year -- Modified By	:	Nilesh Utture on 13-12-13
					 AND IsActive = 1
		END
	ELSE

		---------- If NSC rejects the target for particular AM then update TC_TMAMTargetChangeMaster for that-------
		---------- AM if that record is active (i.e. IsActive = 1) & make IsActive = 0 for that record-------------

		  BEGIN
			  UPDATE TC_TMAMTargetChangeMaster
			  SET    IsAprrovedByNSC = 0,
					 IsActive = 0,
					 NSCActionDate = GETDATE(),
					 NSCId = @AprroversId,
					 NSCComments = @Comments -- Modified By  :   Nilesh Utture on 17th Dec, 2013
			  WHERE  TC_AMId = @TC_AMId
					 AND [Year] = @Year -- Modified By	:	Nilesh Utture on 13-12-13
					 AND IsActive = 1
		  END  
END  
 
