IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetInquiryPoint]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetInquiryPoint]
GO

	-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 26-11-2015
-- Description:	Get Inquiry Point Categories
-- Modified by: Kritika Choudhary on 16th Dec 2015, removed P.InqPtCategoryId from select and added case condition for pitching amount
-- Modified By: Kritika Choudhary on 17th Dec 2015, added @DealerTypeId paramete and condition for @DealerTypeId and GroupType
-- Modified By: Kritika Choudhary on 20th Jan 2016, cast PitchingAmount to float
-- [DCRM_GetInquiryPoint] 1
-- Modified By : Sunil M. Yadav On 02 june 2016
-- Description : accept @DealerId and get packages based on dealer type and ApplicationTypeId.
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetInquiryPoint] 
  @DealerTypeId		INT = NULL,
  @DealerId INT = NULL
  AS
BEGIN

	DECLARE @ApplicationId INT -- , @DealerTypeId INT 
	IF(@DealerId IS NOT NULL AND @DealerId > 0)
	BEGIN
		SELECT @ApplicationId = ApplicationId ,@DealerTypeId = TC_DealerTypeId FROM Dealers WITH(NOLOCK) WHERE ID = @DealerId
	
		IF (@ApplicationId = 1)									-- @ApplicationId = 1 : Carwale dealer
			BEGIN
				SELECT    IPC.Id ProductId ,IPC.Name ProductName,P.Id SubProductId,P.Name SubProductName ,CASE WHEN P.Validity = 0 THEN cast(P.Amount as numeric(15,2)) ELSE  CAST (cast(P.Amount as FLOAT)/cast(P.Validity AS float) AS numeric(15,2)) END AS PitchingAmount
				FROM      InquiryPointCategory  IPC(NOLOCK) 
				JOIN      Packages P(NOLOCK) ON IPC.Id=P.InqPtCategoryId
				WHERE     IPC.isActive = 1 AND P.ForDealer = 1 AND P.IsActive = 1 
						  AND ((@DealerTypeId = 3 AND IPC.GroupType IN (1,2,3)) OR (@DealerTypeId <> 3 AND IPC.GroupType IN (@DealerTypeId,3))) 
	         
				ORDER BY  IPC.Name 
			END
		ELSE 
			IF(@ApplicationId = 2)								-- @ApplicationId = 1 : BikeWale dealer
			BEGIN
				SELECT    IPC.Id ProductId ,IPC.Name ProductName,P.Id SubProductId,P.Name SubProductName ,CASE WHEN P.Validity = 0 THEN cast(P.Amount as numeric(15,2)) ELSE  CAST (cast(P.Amount as FLOAT)/cast(P.Validity AS float) AS numeric(15,2)) END AS PitchingAmount
				FROM      InquiryPointCategory  IPC(NOLOCK) 
				JOIN      Packages P(NOLOCK) ON IPC.Id=P.InqPtCategoryId
				WHERE     IPC.isActive = 1 AND P.ForDealer = 1 AND P.IsActive = 1 
						  AND IPC.GroupType = 5					-- IPC.GroupType = 5 : BikeWale packages
	         
				ORDER BY  IPC.Name
			END
	END
	ELSE
	BEGIN
		SELECT    IPC.Id ProductId ,IPC.Name ProductName,P.Id SubProductId,P.Name SubProductName ,CASE WHEN P.Validity = 0 THEN cast(P.Amount as numeric(15,2)) ELSE  CAST (cast(P.Amount as FLOAT)/cast(P.Validity AS float) AS numeric(15,2)) END AS PitchingAmount
				FROM      InquiryPointCategory  IPC(NOLOCK) 
				JOIN      Packages P(NOLOCK) ON IPC.Id=P.InqPtCategoryId
				WHERE     IPC.isActive = 1 AND P.ForDealer = 1 AND P.IsActive = 1 
						  AND ((@DealerTypeId = 3 AND IPC.GroupType IN (1,2,3)) OR (@DealerTypeId <> 3 AND IPC.GroupType IN (@DealerTypeId,3))) 
	         
				ORDER BY  IPC.Name 
	END

END

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
