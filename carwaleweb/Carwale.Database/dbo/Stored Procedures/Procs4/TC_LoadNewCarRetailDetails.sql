IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LoadNewCarRetailDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LoadNewCarRetailDetails]
GO

	
-- =============================================
-- Author:		Vivek Gupta
-- Create date: 04-09-2013
-- Description:	loading retail invoice details for new car inquiries for editing.
-- Modified By ; Tejashree Patil on 25 Sept 2013, Added query to get Chassis Number, color list for booking.
-- Modified By ; Tejashree Patil on 7 Nov 2013, Added @MakeId parameter and Fetched VW Offers and Payment Mode.
-- [TC_LoadNewCarRetailDetails] 7454,null,1028,20
-- [TC_LoadNewCarRetailDetails] 7559,4670,1028,20
-- [TC_LoadNewCarRetailDetails] 7570,203,1028,20
-- TC_LoadNewCarRetailDetails 7483,203,1028,20
-- Modified By : Tejashree Patil on 16 Dec 2013, Uncommented chassis number related changes and implemented sharing dealers query.
-- Modified By : Tejashree Patil on 24 Dec 2013, Changed query to get chassis numbers.
-- =============================================
CREATE PROCEDURE [dbo].[TC_LoadNewCarRetailDetails]
	@InqId BIGINT,
	@LeadOwnerId INT = NULL,
	@BranchId INT = NULL,
	@MakeId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
    
    IF (@InqId IS NOT NULL)
    BEGIN
		-- Modified By : Tejashree Patil on 16 Dec 2013, Uncommented chassis number related changes and implemented sharing dealers query.
		DECLARE @VersionCode VARCHAR(15),@HexCode VARCHAR(15),@VersionYear VARCHAR(15),@ChassisNumber VARCHAR(18), @Color VARCHAR(20)
	
		   SELECT	   @VersionCode=VC.CarVersionCode,
				       @HexCode=V.HexCode,
				       @VersionYear = SI.ModelYear,
					   @ChassisNumber = B.ChassisNumber,
					   @Color = V.Color
		FROM	TC_NewCarInquiries NI
				INNER JOIN	TC_NewCarBooking B WITH(NOLOCK)
							                         ON	B.TC_NewCarInquiriesId=NI.TC_NewCarInquiriesId
			    INNER JOIN TC_VersionsCode AS VC WITH(NOLOCK) ON VC.CarVersionId=NI.VersionId
				LEFT JOIN	TC_StockInventory SI WITH(NOLOCK)
										    		 ON	B.ChassisNumber=SI.ChassisNumber
				LEFT JOIN	TC_vwVersionColorCode V WITH(NOLOCK) 
							                         ON   SI.ModelCode=V.CarVersionCode 
							                          AND SI.ColourCode=V.ColorCode 
							                           AND V.IsActive=1
		        WHERE	NI.TC_NewCarInquiriesId  = @InqId
	
			
		--DECLARE @VersionCode VARCHAR(15)
		SELECT	@VersionCode=VC.CarVersionCode
		FROM	TC_NewCarInquiries NI
				INNER JOIN	TC_VersionsCode VC WITH(NOLOCK) 
							ON VC.CarVersionId=NI.VersionId
		WHERE	NI.TC_NewCarInquiriesId = @InqId
		------------------------------------------------------------------------------------------------------------------------------------------
		SELECT	InvoiceDate , Salutation, BookingName, LastName, BookingMobile, ChassisNumber , BookingDate, 
				TC_PaymentModeId , TC_OffersId, @VersionCode VersionCode,@HexCode HexCode,@VersionYear VersionYear, @Color Color
				-- Modified By ; Tejashree Patil on 7 Nov 2013, Added @MakeId parameter and Fetched VW Offers and Payment Mode.
		FROM	TC_NewCarBooking WITH(NOLOCK)
		WHERE	TC_NewCarInquiriesId = @InqId
		
		--Get Chassis Numbers which are available.
		--------------------------------------------------------------------------------------------------------
		-- Modified By : Tejashree Patil on 24 Dec 2013, Changed query to get chassis numbers.
		DECLARE @DealerIds VARCHAR(MAX)=@BranchId, @MappingId VARCHAR(MAX)

		SELECT	@MappingId = COALESCE(@MappingId+',','')+(CONVERT(VARCHAR(50),D.TC_DealerMappingId))
		FROM	TC_SubDealers AS S  WITH(NOLOCK)
				JOIN	TC_DealerMapping AS D  WITH(NOLOCK) 
						ON S.TC_DealerMappingId=D.TC_DealerMappingId
		WHERE	(D.DealerAdminId =@BranchId OR S.SubDealerId = @BranchId)
				AND  S.IsActive=1 AND  D.IsActive=1
				
		SELECT	@DealerIds = COALESCE(@DealerIds+',','')+(CONVERT(VARCHAR(50),D.DealerAdminId))+','+(CONVERT(VARCHAR(50),S.SubDealerId))
		FROM	TC_SubDealers AS S  WITH(NOLOCK)
				JOIN	TC_DealerMapping AS D  WITH(NOLOCK) 
						ON S.TC_DealerMappingId=D.TC_DealerMappingId
		WHERE	(S.TC_DealerMappingId IN (SELECT ListMember FROM fnSplitCSV(@MappingId)))
				AND  S.IsActive=1
				AND  D.IsActive=1
				
		SELECT  ChassisNumber
		FROM    TC_StockInventory WITH(NOLOCK)
		WHERE	ModelCode=@VersionCode
				AND (BranchId IN (SELECT ListMember FROM fnSplitCSV(@DealerIds)))-- Modified By : Tejashree Patil on 24 Dec 2013, Changed query to get chassis numbers.
		----------------------------------------------------------------------------------------------------------
		/*
		SELECT  ChassisNumber
		FROM    TC_StockInventory WITH(NOLOCK)
		WHERE	ModelCode=@VersionCode
				AND (BranchId IN (@BranchId) OR BranchId IN (	SELECT	SubDealerId 
															FROM	TC_SubDealers AS S  WITH(NOLOCK)
																	JOIN	TC_DealerMapping AS D  WITH(NOLOCK) 
																			ON S.TC_DealerMappingId=D.TC_DealerMappingId
															WHERE	D.DealerAdminId=@BranchId
																	AND  S.IsActive=1 
																	AND  D.IsActive=1)-- Modified By:Tejashree Patil on 16 Dec 2013, implemented sharing inventory dealers query.
				    )
		*/
		EXCEPT
		SELECT  ChassisNumber
		FROM    TC_NewCarBooking WITH(NOLOCK)
		WHERE   ChassisNumber IS NOT NULL
				--AND TC_NewCarInquiriesId = @InqId
		
		-- Modified By ; Tejashree Patil on 7 Nov 2013, Added @MakeId parameter and Fetched VW Offers and Payment Mode.
		SELECT	TC_PaymentModeId, PaymentModeName
		FROM	TC_PaymentMode WITH(NOLOCK)
		WHERE	MakeId=@MakeId
				AND IsActive = 1
		
		SELECT	TC_OffersId , OfferName
		FROM	TC_Offers WITH(NOLOCK)
		WHERE	MakeId=@MakeId
				AND IsActive = 1
				AND TC_OffersType = 1


    END

END
/****** Object:  StoredProcedure [dbo].[TC_CaptureInvoiceDate]    Script Date: 12/27/2013 14:19:20 ******/
SET ANSI_NULLS ON
