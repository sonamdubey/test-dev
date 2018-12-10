IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetInquiryPoint]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetInquiryPoint]
GO

	

-- =============================================
-- Author	:	Sachin Bharti
-- Create date	:	16th Dec 2013
-- Description	:	Get Inquiry Point Categories
-- Modifier	:	Sachin Bharti(15th Jan 2015)
-- Purpose	:	Add query for get dealer product status
-- Modifier	:	Sachin Bharti(21st Jan 2015)
-- Purpose	:	Added query for campaign type
--Modifier  : Ajay Singh(4 dec 2015)
--Description : Added a parameter DealerId for change inqpoint
--Modifier : Vaibhav K 6-Jan-2015
--Included new ProductStatus from table DCRM_ADM_ProductStatus (6,CarTrade Migration)
--Modifier	:	Amit Yadav 
--Purpose	:	To check if the dealer is a group or outlet or multioutlet.
-- EXEC [M_GetInquiryPoint] 11972
--Modifier : Kartik Rathod on 9 Mar 2016 fetche IsActive for ClosingProbability 
-- =============================================

CREATE PROCEDURE [dbo].[M_GetInquiryPoint]
      @DealerId INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	DECLARE @DealerTypeId TINYINT
	DECLARE @ApplicationId INT
	SELECT @DealerTypeId = TC_DealerTypeId, @ApplicationId = AD.ApplicationId FROM Dealers AS AD WITH(NOLOCK) WHERE AD.ID=@DealerId
	
	--get all inquiry points
	IF @ApplicationId = 1
	BEGIN
		IF @DealerTypeId = 1
			BEGIN
				SELECT IPC.Id ,IPC.Name 
				FROM InquiryPointCategory  IPC(NOLOCK) 
				WHERE IPC.isDealer = 1 AND IPC.isActive = 1 AND IPC.GroupType IN(1,3)
				ORDER BY Name 
			END
		ELSE IF @DealerTypeId = 2
			BEGIN
				SELECT IPC.Id ,IPC.Name 
				FROM InquiryPointCategory  IPC(NOLOCK) 
				WHERE IPC.isDealer = 1 AND IPC.isActive = 1 AND IPC.GroupType IN(2,3)
				ORDER BY Name 
			END
		ELSE IF @DealerTypeId = 3
			BEGIN
				SELECT IPC.Id ,IPC.Name 
				FROM InquiryPointCategory  IPC(NOLOCK) 
				WHERE IPC.isDealer = 1 AND IPC.isActive = 1 AND IPC.GroupType IN(1,2,3)
				ORDER BY Name 
			END
	END
	ELSE
	--For bikewale dealers.
	 IF @ApplicationId = 2 
		BEGIN
			SELECT IPC.Id ,IPC.Name 
			FROM InquiryPointCategory  IPC(NOLOCK) 
			WHERE IPC.isDealer = 1 AND IPC.isActive = 1 AND IPC.GroupType = 5
			ORDER BY Name 
		END

	
	
	--get dealers product status
	SELECT DAP.ID AS VALUE, DAP.Name AS TEXT 
	FROM DCRM_ADM_ProductStatus DAP(NOLOCK)
	WHERE DAP.ID IN (2,3,5,6)
	ORDER BY DAP.Name ASC

	--get packages closing stages
	SELECT PC.ClosingStage AS VALUE,CONVERT(VARCHAR,PC.ClosingStage)+'% - '+PC.Description AS TEXT ,PC.IsActive 
	FROM DCRM_PackagesClosingStage PC(NOLOCK)
	WHERE PC.ClosingStage <> 100
	ORDER BY VALUE

	--get all campaign types
	SELECT DCT.Id AS VALUE , DCT.CampaignType AS TEXT
	FROM DCRM_CampaignType DCT(NOLOCK)
	WHERE DCT.Id <> 2
	ORDER BY TEXT 

	--Check if the dealer is group or outlet
	SELECT D.IsGroup AS IsGroup, D.IsMultiOutlet AS MultiOutlet FROM Dealers D(NOLOCK) 
	WHERE D.ID=@DealerId
	
END


