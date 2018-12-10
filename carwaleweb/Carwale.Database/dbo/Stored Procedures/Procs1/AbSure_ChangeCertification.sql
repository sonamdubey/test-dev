IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_ChangeCertification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_ChangeCertification]
GO

	-- Author		:	Tejashree Patil.
-- Create date	:	16 Feb 2015
-- Description	:1. If score is >= 60% Change the certification logo url with absure logo url at both live listing and sell inquiries table
--				 2. if score < 60% check if the stock id already have absure logo url, replace it with dealers certification url else no changes.
-- Modifier 1: Ruchira Patil on (27th Feb 2015) - updated AbsureScore and AbsureWarrantyType in livelistings
-- Modified By Tejashree Patil on 13 March 2015, Selected only IsActive = 1 cars.
-- Modified By Tejashree Patil on 16 Jun 2015, approved or rejected cars on CarWale.
-- EXECUTE [AbSure_ChangeCertification] 611031,NULL,NULL
-- Modified by Tejashree Patil on 08 JuL 2015 Added where conditions.
-- Modified by Tejashree Patil on 10 JuL 2015 Added if exists  conditions.
-- Modified By: Ashwini Dhamankar on Oct 27,2015 (Because of Duplicate car there could be 2 stocks of same stockid whose isactive = 1 so avoid duplicate car of that stockid)
-- Modified  By : Ashwini Dhamankar on Nov 4,2015 (Added parameter and condition of @IsDuplicateCarSold)
-- Modified By : Chetan Navin on Feb 10 2016 (Added condition to handle in case of cartrade certification)
-- Modified By : Chetan Navin on Mar 10 2016 (Changed cartrade certification expiry days from 90 to 45)
-- =============================================    

CREATE PROCEDURE [dbo].[AbSure_ChangeCertification] 
	@StockId INT  = NULL, 
	@CarScore INT  = NULL,
	@AbSure_CarId INT = NULL,
	@IsDuplicateCarSold BIT = NULL 
AS
BEGIN
	
	DECLARE @AbSureCertificationLogoUrl		VARCHAR(150), 
			@DealerCertificationId			INT , 
			@DealerLogoURL					VARCHAR(250),
			@AbSureCertificationId			INT = 417,
			@WarrantyId						INT,
			@WarrantyName					VARCHAR(50),
			@InspectionCertificationLogoURL	VARCHAR(250),
			@InspectionCertificationId		INT = 454,
			@CarCondition					INT,
			@Status							SMALLINT,
			@SurveyDate						DATETIME
	
	IF NOT EXISTS(SELECT 1 FROM TC_CarTradeCertificationLiveListing WITH (NOLOCK) WHERE ListingId = @StockId)
	BEGIN
		DECLARE @CancelledReason VARCHAR(250)       --Added by : Ashwini Dhamankar on Oct 27,2015
		SELECT @CancelledReason = Reason FROM AbSure_ReqCancellationReason WITH(NOLOCK) WHERE Id = 7; 
	
		SELECT  TOP 1
				@CarScore					= CarScore,
				@AbSure_CarId				= Id,
				@StockId					= StockId,
				@WarrantyId					= FinalWarrantyTypeId,
				@WarrantyName				= (CASE  
													 WHEN  CD.Status = 2 THEN 'Rejected'
													 ELSE WT.Warranty 
											   END),
				@Status						= CD.[status],
				@SurveyDate					= CD.SurveyDate
		FROM	AbSure_CarDetails CD WITH(NOLOCK)
				LEFT JOIN	AbSure_WarrantyTypes WT WITH(NOLOCK) 
							ON WT.AbSure_WarrantyTypesId = CD.FinalWarrantyTypeId AND WT.IsActive = 1
		WHERE	
		(
				(Id = @AbSure_CarId
				OR	StockId = @StockId)
				AND CD.IsActive = 1
				AND (CD.CancelReason <> @CancelledReason OR  CD.CancelReason IS NULL)    --Because of Duplicate car there could be 2 stocks of same stockid whose isactive = 1 so avoid duplicate car of that stockid
		)
	
		IF EXISTS (SELECT 1 FROM AbSure_CarDetails WITH(NOLOCK) WHERE StockId = @StockId  OR ISNULL(@IsDuplicateCarSold,0) = 1)
		BEGIN
			SELECT  @DealerCertificationId = CertificationId, 
					@DealerLogoURL		   = CertifiedLogoUrl
			FROM	TC_Stock	WITH(NOLOCK) 
			WHERE	Id = @StockId
					AND IsActive = 1
				
			SELECT  @AbSureCertificationLogoUrl = HostURL + DirectoryPath + LogoURL 
			FROM	Classified_CertifiedOrg		WITH(NOLOCK) 
			WHERE	Id = @AbSureCertificationId

			--SELECT  @InspectionCertificationLogoURL = HostURL + DirectoryPath + LogoURL 
			--FROM	Classified_CertifiedOrg		WITH(NOLOCK) 
			--WHERE	Id = @InspectionCertificationId	

			IF(@CarScore >= 60 AND (@status IN (4,8,2) OR DATEDIFF(DAY,@SurveyDate,GETDATE()) > 30) AND @status NOT IN (1,3,5,6))
			BEGIN
				UPDATE  SI
				SET     SI.CertifiedLogoUrl        = CASE	--WHEN CD.FinalWarrantyTypeId = 1 AND ((cd.[status] IN (4) AND DATEDIFF(DAY,CD.SurveyDate,GETDATE()) <= 30)  OR (cd.[status] IN (8))) 
															WHEN CD.FinalWarrantyTypeId = 1 AND (DATEDIFF(DAY,CD.SurveyDate,GETDATE()) <= 30 OR (DATEDIFF(DAY,CD.SurveyDate,GETDATE()) > 30 AND CD.Status = 8))
															THEN @AbSureCertificationLogoUrl 
															ELSE @InspectionCertificationLogoURL 
													 END,
						SI.CertificationId         = CASE	--WHEN CD.FinalWarrantyTypeId = 1 AND ((cd.[status] IN (4) AND DATEDIFF(DAY,CD.SurveyDate,GETDATE()) <= 30)  OR (cd.[status] IN (8))) 
															WHEN CD.FinalWarrantyTypeId = 1 AND (DATEDIFF(DAY,CD.SurveyDate,GETDATE()) <= 30 OR (DATEDIFF(DAY,CD.SurveyDate,GETDATE()) > 30 AND CD.Status = 8))
															THEN @AbSureCertificationId 
															ELSE @InspectionCertificationId  
													 END
				FROM    SellInquiries SI						 WITH(NOLOCK)
						INNER JOIN livelistings L				 WITH(NOLOCK) ON L.Inquiryid = SI.ID AND L.SellerType = 1
						INNER JOIN AbSure_CarDetails CD          WITH (NOLOCK) ON CD.StockId = SI.TC_StockId AND CD.IsActive = 1 AND CD.ID=@AbSure_CarId
						LEFT  JOIN AbSure_WarrantyTypes    WT    WITH (NOLOCK) ON WT.AbSure_WarrantyTypesId = CD.FinalWarrantyTypeId
						LEFT  JOIN Absure_WarrantyStatuses ST    WITH (NOLOCK) ON ST.Id = CD.Status
						WHERE CD.ID=@AbSure_CarId

				UPDATE  L
				SET     L.AbsureScore         =    CD.CarScore,
						L.AbsureWarrantyType  =    CASE WHEN  CD.Status = 2 THEN 'Rejected' ELSE WT.Warranty END
				FROM    SellInquiries SI                        WITH (NOLOCK)
						INNER JOIN livelistings L               WITH (NOLOCK) ON L.Inquiryid = SI.ID AND L.SellerType = 1 
						INNER JOIN AbSure_CarDetails CD         WITH (NOLOCK) ON CD.StockId = SI.TC_StockId AND CD.IsActive = 1 AND CD.ID=@AbSure_CarId
						LEFT  JOIN AbSure_WarrantyTypes    WT   WITH (NOLOCK) ON WT.AbSure_WarrantyTypesId = CD.FinalWarrantyTypeId
						LEFT  JOIN Absure_WarrantyStatuses ST   WITH (NOLOCK) ON ST.Id = CD.Status
						WHERE CD.ID=@AbSure_CarId

			END
			ELSE
			BEGIN
				UPDATE	SI 
				SET		SI.CertifiedLogoUrl		= @DealerLogoURL, 
						SI.CertificationId		= @DealerCertificationId
				FROM	SellInquiries SI			WITH(NOLOCK)
						INNER JOIN livelistings L	WITH(NOLOCK) ON L.Inquiryid = SI.ID AND L.SellerType = 1
				WHERE	SI.TC_StockId = @StockId
			END
			--create log
			INSERT INTO AbSure_LiveListingUpdateLog (CertifiedLogoUrl,CertificationId,AbSure_CarDetailsId,AbSure_Score,TC_StockId, EntryDate, AbSure_WarrantyTypeId)
			VALUES (ISNULL(@AbSureCertificationLogoUrl,@InspectionCertificationLogoURL),ISNULL(@AbSureCertificationId,@InspectionCertificationId),@AbSure_CarId,@CarScore,@StockId, GETDATE(),@WarrantyId)	
		END
	END
	ELSE 
	BEGIN
		-- Executes if car is already live/ going live
		-- Check for CarTrade Certification
		-- If Warranty is available consider CarWale Guarranty Logo and Certification Id 417
		-- If only certification consider No logo and Certification Id 454
		
		DECLARE @TC_CarTradeCertificationDataId INT;
		SELECT @TC_CarTradeCertificationDataId = TC.TC_CarTradeCertificationDataId, @AbSureCertificationId =(CASE ISNULL(TC.IsWarranty,0) WHEN 1 THEN 417 ELSE 454 END)
		FROM TC_CarTradeCertificationData TC WITH(NOLOCK) INNER JOIN TC_CarTradeCertificationLiveListing TL WITH(NOLOCK) ON TC.ListingId = TL.ListingId
		WHERE TC.ListingId = @StockId AND DATEDIFF(DAY,TC.InvCertifiedDate,GETDATE()) <= 45
		
		--If Certification exists against TCStockId
		IF @@ROWCOUNT > 0
			BEGIN
				-- Car is already live
				-- Get the logo URL as per the certification ID
				SELECT  @AbSureCertificationLogoUrl = (CASE @AbSureCertificationId WHEN 417 THEN HostURL + DirectoryPath + LogoURL  ELSE NULL END)
				FROM	Classified_CertifiedOrg		WITH(NOLOCK) 
				WHERE	Id = @AbSureCertificationId	

				
				--Update Sell Inquiries data
				UPDATE  SI
				SET     SI.CertifiedLogoUrl = @AbSureCertificationLogoUrl,
						SI.CertificationId  = @AbSureCertificationId
				FROM    SellInquiries SI						 WITH(NOLOCK)
						INNER JOIN livelistings L				 WITH(NOLOCK) ON L.Inquiryid = SI.ID AND L.SellerType = 1				
						WHERE SI.TC_StockId = @StockId
			END
		ELSE
			BEGIN
					-- If car was unavailable and going live and certification is expired
					
					-- Get dealer certification details if dealer have certification
					SELECT  @DealerCertificationId = CertificationId, @DealerLogoURL = CertifiedLogoUrl
					FROM	TC_Stock	WITH(NOLOCK) 
					WHERE	Id = @StockId

							
					UPDATE	SI 
					SET		SI.CertifiedLogoUrl		= @DealerLogoURL, 
							SI.CertificationId		= @DealerCertificationId
					FROM	SellInquiries SI			WITH(NOLOCK)
							INNER JOIN livelistings L	WITH(NOLOCK) ON L.Inquiryid = SI.ID AND L.SellerType = 1
					WHERE	SI.TC_StockId = @StockId
			END
			--create log
			INSERT INTO AbSure_LiveListingUpdateLog (CertifiedLogoUrl,CertificationId,AbSure_CarDetailsId,AbSure_Score,TC_StockId, EntryDate, AbSure_WarrantyTypeId)
			VALUES (ISNULL(@AbSureCertificationLogoUrl,@InspectionCertificationLogoURL),ISNULL(@AbSureCertificationId,@InspectionCertificationId),@TC_CarTradeCertificationDataId,@CarScore,@StockId, GETDATE(),@WarrantyId)	
	END
END
-------------------------------------------------------------------------------------------------------------------
