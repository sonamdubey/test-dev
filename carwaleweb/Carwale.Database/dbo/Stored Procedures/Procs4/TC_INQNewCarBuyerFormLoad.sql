IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQNewCarBuyerFormLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQNewCarBuyerFormLoad]
GO

	-- =============================================  
-- Author:  Tejashree Patil  
-- Create date: 9 Jan 2013  
-- Description: To bind controls on New Car Buyer Inquiry form load
-- EXECUTE TC_INQNewCarBuyerFormLoad 5,20  
-- Modified By : Umesh Ojha on 27 june 2013 for fetching inquiry source as per dealer make wise
-- Modified By: Vivek Gupta on 23rd july, Added Make Model Version parameters to be fetched.
-- Modified By Vivek on 31st july,2013 , changed TC_InquirySourceId to CD.TC_InquirySourceId
-- Modified By Vivek Gupta on 7th Aug,2013 Added @iscorporate and @companyName parameters
-- Modified By Tejashree Patil on 2 Sept,2013 Fetched full name of customer.
-- Modified By Vivek Gupta on 10th Sep,2013 fetching  InquiryAddedDate 
-- Modified By : Vivek Gupta on 4thoct,2013 added condition for version colors isactive=1
-- Modified By : Tejashree Patil on 12 Oct 2013, Fetched campaign details.
-- TC_INQNewCarBuyerFormLoad 1028,7279,7484
--Modified By Vishal Srivastava AE1830 03-04-2014 1814 hrs ist added isexchage for pre selected field
-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application and joined with vwAllMMV view.
-- =============================================  
CREATE  PROCEDURE [dbo].[TC_INQNewCarBuyerFormLoad]
 @BranchId BIGINT ,
 @CustomerId BIGINT = NULL,
 @TC_NewCarInquiriesId BIGINT = NULL,
 @ApplicationId TINYINT = NULL
AS  
BEGIN  
	 SET NOCOUNT ON;
	 
	 EXECUTE TC_DealerCitiesView @BranchId
	 --EXECUTE TC_InquirySourceSelect
	 EXECUTE TC_InquirySourceDealerWise @BranchId
	 EXECUTE TC_InquiryStatusSelect
	 
	 DECLARE @VersionId BIGINT = NULL
	 DECLARE @Version VARCHAR(50) = NULL
	 DECLARE @MakeId BIGINT = NULL
	 DECLARE @Make VARCHAR(50) = NULL
	 DECLARE @ModelId BIGINT = NULL
	 DECLARE @Model VARCHAR(50) = NULL
	 DECLARE @CityId VARCHAR(50) = NULL
	 DECLARE @IsCorporate TINYINT = NULL
	 DECLARE @CompanyName VARCHAR(100) = NULL
	 DECLARE @CorporateListId INT = NULL
	 DECLARE @InquiryAddedDate DATETIME = NULL
	 DECLARE @CampaignId INT = NULL
	 DECLARE @CampaignName VARCHAR(200) = NULL
	 DECLARE @IsExchange TINYINT--Modified By Vishal Srivastava AE1830 03-04-2014 1814 hrs ist added isexchage for pre selected field
	 
	 IF(@TC_NewCarInquiriesId IS NOT NULL)
	 BEGIN
		 SELECT @VersionId=TCN.VersionId , @CityId = TCN.CityId , @IsCorporate = TCN.IsCorporate , @CompanyName = TCN.CompanyName, 
		 @InquiryAddedDate = TCN.CreatedOn , ----Modified by Vivek on 10th Sep,2013 fetching InquiryAddedDate 
		 @CampaignId = TCN.TC_CampaignSchedulingId, @CampaignName = SC.SubCampaignName,-- Modified By : Tejashree Patil on 12 Oct 2013, Fetched campaign details.
		 @IsExchange = TCN.TC_NewCarExchangeId--Modified By Vishal Srivastava AE1830 03-04-2014 1814 hrs ist added isexchage for pre selected field
		 FROM TC_NewCarInquiries TCN WITH(NOLOCK)
		 LEFT JOIN TC_SubCampaign SC WITH(NOLOCK) ON SC.TC_SubCampaignId=tcn.TC_CampaignSchedulingId
		 WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId
		 
		 -- Modified By : Tejashree Patil on 30-10-2014, Commented query and joined with vwAllMMV view.
		 /*SELECT @Make = v.Make,@MakeId = v.MakeId , @ModelId = v.ModelId, @Model = v.Model
		 FROM vwMMV v WITH(NOLOCK)
		 WHERE v.VersionId = @VersionId*/
		 SELECT @Make = v.Make,@MakeId = v.MakeId , @ModelId = v.ModelId, @Model = v.Model
		 FROM	vwAllMMV v WITH(NOLOCK)
		 WHERE	v.VersionId = @VersionId
				AND v.ApplicationId=@ApplicationId
		 SELECT @CorporateListId = TC_CorporateListId FROM TC_CorporateList WHERE Name LIKE '%' + @CompanyName + '%'
	 END

	 
	 IF(@CustomerId IS NOT NULL)
	 BEGIN
	    IF(@TC_NewCarInquiriesId IS NOT NULL)
			BEGIN
				SELECT	CustomerName,Mobile,Email, Address,INQL.TC_InquiryStatusId Eagerness,@CityId AS CityId,@MakeId AS MakeId, @ModelId AS ModelId,
						@VersionId AS VersionId,CD.TC_InquirySourceId AS Source,Buytime,Comments, ---- Modified By Vivek on 31st july,2013
						@IsCorporate AS IsCorporate, @CompanyName AS CompanyName, @CorporateListId AS CorporateId,LastName,Salutation, Address, 
						@InquiryAddedDate AS InquiryDate,-- Modified By Tejashree Patil on 2 Sept,2013 -- Modified By Vivek Gupta on 10th Sep,2013 Added @InquiryAddedDate 
						@CampaignId AS CampaignId,
						@IsExchange AS Exchange --Modified By Vishal Srivastava AE1830 03-04-2014 1814 hrs ist added isexchage for pre selected field
				FROM	TC_CustomerDetails CD WITH (NOLOCK) 
						INNER JOIN	TC_InquiriesLead INQL WITH(NOLOCK) ON
									CD.Id=INQL.TC_CustomerId
				WHERE	CD.BranchId=@BranchId AND Id=@CustomerId
				AND INQL.TC_LeadStageId <>3
			END
		ELSE
		    BEGIN
				SELECT	CustomerName,Mobile,Email, Address,INQL.TC_InquiryStatusId Eagerness,@CityId AS CityId,@MakeId AS MakeId, @ModelId AS ModelId,
						@VersionId AS VersionId,'-1' AS Source,'-1' AS Buytime, Comments, ---- Modified By Vivek on 31st july,2013
						@IsCorporate AS IsCorporate , @CompanyName AS CompanyName, @CorporateListId AS CorporateId,LastName,Salutation, Address , 
						@InquiryAddedDate AS InquiryDate,-- Modified By Tejashree Patil on 2 Sept,2013 -- Modified By Vivek Gupta on 10th Sep,2013 Added @InquiryAddedDate 
						@CampaignId AS CampaignId,
						@IsExchange AS Exchange --Modified By Vishal Srivastava AE1830 03-04-2014 1814 hrs ist added isexchage for pre selected field 
				FROM	TC_CustomerDetails CD WITH (NOLOCK) 
						INNER JOIN	TC_InquiriesLead INQL WITH(NOLOCK) ON
									CD.Id=INQL.TC_CustomerId
				WHERE	CD.BranchId=@BranchId AND Id=@CustomerId
				AND INQL.TC_LeadStageId <>3
			END
		-----------------------------------------------------Below Table will give Versions of the Model.
		IF(@TC_NewCarInquiriesId IS NOT NULL)
		BEGIN
			/*
			SELECT ID AS Value, Name AS Text 
			FROM CarVersions WITH(NOLOCK)
			WHERE CarModelId = @ModelId AND
			IsDeleted = 0 
			AND Futuristic = 0 
			AND New = 1 
			ORDER BY Text  
			*/

			SELECT	V.VersionId AS Value, V.Version AS Text 
			FROM	vwAllMMV V WITH(NOLOCK)
			WHERE	V.ModelId = @ModelId
					AND V.New = 1 
					AND V.ApplicationId = @ApplicationId
			ORDER BY Text  
			-----------------------------------------------------Below Table will give Model of the Make.
			/*SELECT ID AS Value, Name AS Text 
			FROM CarModels WITH(NOLOCK)
			WHERE CarMakeId = @MakeId AND
			IsDeleted = 0 
			AND Futuristic = 0 
			AND New = 1 
			ORDER BY Text  */
			SELECT @ModelId AS Value, @Model AS Text 
			-----------------------------------------------------Below Table will give Versions of the Model.
			-- Modified By : Vivek Gupta on 4thoct,2013 added condition for version colors isactive=1
			/*SELECT V.ID, V.Color 
			FROM VersionColors V WITH(NOLOCK)
			WHERE CarVersionID = @VersionId AND IsActive=1*/

			SELECT	V.VersionColorsId ID, V.VersionColor Color
			FROM	vwAllVersionColors V WITH(NOLOCK)
			WHERE	V.VersionId = @VersionId
					AND V.ApplicationId = @ApplicationId
			
			-----------------------------------------------------Below Table will give ColorsId of the Versions.
			SELECT TCPC.VersionColorsId 
			FROM TC_PrefNewCarColour TCPC WITH(NOLOCK)
			WHERE TCPC.TC_NewCarInquiriesId = @TC_NewCarInquiriesId

			-- Modified By : Tejashree Patil on 12 Oct 2013, Fetched campaign details.
			EXEC TC_CheckCampiagnsAvailability @CityId, @InquiryAddedDate ,@ModelId
		END
	 END
END

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


