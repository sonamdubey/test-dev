IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetCarWaleDealerContact]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetCarWaleDealerContact]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 09-JUn=2014
-- Description:	Get the share details || Email, SMS
--Modified:ADDed Check for duplicate entry in Noncarwale ||24-Juj --Ranjeet
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetCarWaleDealerContact] 
	@DealerIdList VARCHAR(500),
	@GroupIdList VARCHAR(100),
	@NonCarWaleNameList VARCHAR(500),
	@NonCarWaleMobileList VARCHAR(1000),
	@StockId INT	
AS
BEGIN
	SET NOCOUNT ON;
	

	--Insert Groups data
	INSERT INTO BA_GroupSharing (StockId,GroupDetailsID,Url,CreatedOn) 
				SELECT @StockId,groupDetails.ID,NULL, GETDATE() FROM  BA_Groups AS groups WITH(NOLOCK)
				INNER JOIN  BA_GroupDetails AS groupDetails WITH(NOLOCK) ON groupDetails.GroupId = groups.Id 
				INNER JOIN fnSplitCSV(@GroupIdList) AS  groupList  ON groups.ID = groupList.ListMember
	--Insert to NonCaRwale
	INSERT INTO [dbo].[BA_NonCarWaleDealer]
			   ([Mobile]
			   ,[Name]
			   ,[Address]
			   ,[CreatedDate]
			   ,[ModifyDate]
			   ,[IsActive]
			   ,[BrokerId])
			   SELECT mobiles.ListMember, names.ListMember, NULL, GETDATE(), NULL, 1, stock.BrokerId FROM  [fnSplitCSVValuesWithIdentity](@NonCarWaleNameList) AS names
			   INNER JOIN [fnSplitCSVValuesWithIdentity](@NonCarWaleMobileList) AS mobiles ON mobiles.Id = names.Id
			   INNER JOIN  BA_Stock AS stock WITH(NOLOCK) ON stock.ID = @StockId
			   WHERE NOT EXISTS(SELECT BNC.Mobile
                    FROM BA_NonCarWaleDealer AS BNC WITH(NOLOCK)
                   WHERE BNC.Mobile = mobiles.ListMember)
	--insert NonCarWale details in BA_SharingIndividual
	INSERT INTO [dbo].[BA_SharingIndividual]
			   ([StockId]
			   ,[DealerId]
			   ,[Url]
			   ,[CreatedOn]
			   ,[ModifiedOn]
			   ,[IsCarwaleDealer]
			   ,[IsClicked]
			   ,[Comments])
			   SELECT @StockId,nonCarWale.ID, NULL, GETDATE(), NULL, 0, 0,NULL FROM BA_NonCarWaleDealer AS nonCarWale WITH(NOLOCK)
			   INNER JOIN [fnSplitCSVValuesWithIdentity](@NonCarWaleNameList) AS names ON nonCarWale.Name = names.ListMember
			   INNER JOIN [fnSplitCSVValuesWithIdentity](@NonCarWaleMobileList) AS mobiles ON mobiles.Id = names.Id AND nonCarWale.Mobile = mobiles.ListMember

	INSERT INTO [dbo].[BA_SharingIndividual]
			   ([StockId]
			   ,[DealerId]
			   ,[Url]
			   ,[CreatedOn]
			   ,[ModifiedOn]
			   ,[IsCarwaleDealer]
			   ,[IsClicked]
			   ,[Comments])
			   SELECT @StockId,ids.ListMember, NULL, GETDATE(), NULL, 1, 0,NULL FROM  fnSplitCSV_WithId(@DealerIdList) AS ids 
	--Get Data from carWale dealers 
	SELECT D.MobileNo, D.EmailId,D.ID AS Id FROM Dealers AS D WITH(NOLOCK) WHERE  D.ID In (SELECT DL.ListMember FROM fnSplitCSV(@DealerIdList) AS DL);

	--Get Data from GroupIds
	SELECT nonCarwale.Mobile AS nMobile,nonCarwale.ID AS NId , dealers.MobileNo  AS cMobile, dealers.id as CId FROM BA_GroupDetails  AS groupDetails 
	INNER JOIN fnSplitCSV(@GroupIdList) AS gList ON gList.ListMember = groupDetails.GroupId
	LEFT JOIN BA_NonCarWaleDealer AS nonCarwale WITH(NOLOCK) ON nonCarwale.IsActive = 1 AND groupDetails.DealerId = nonCarwale.ID AND groupDetails.IsCarWaleDealer = 0
	LEFT JOIN Dealers AS dealers WITH(NOLOCK) ON dealers.ID = groupDetails.DealerId AND groupDetails.IsCarWaleDealer = 1 AND dealers.IsDealerActive = 1 AND dealers.IsDealerDeleted = 0

	--Ids for Individual List
	SELECT BNC.ID AS Id, BNC.Mobile AS Mobile FROM  BA_NonCarWaleDealer AS BNC  WITH(NOLOCK)
	INNER JOIN [fnSplitCSVValuesWithIdentity](@NonCarWaleMobileList) AS L ON BNC.Mobile = L.ListMember 

	--Get Broker Name and mobile
	SELECT register.BrokerName AS Name, register.BrokerMobile AS Mobile, CM.Name AS Model, Ck.Name AS Make FROM BA_StockDetails AS stockDetails
	INNER JOIN BA_RegisterBroker AS register WITH(NOLOCK) ON stockDetails.StockId = @StockId AND register.ID = stockDetails.BrokerId
	INNER JOIN CarModels AS CM WITH(NOLOCK) ON CM.ID = stockDetails.CarModelId
	INNER JOIN CarMakes AS CK WITH(NOLOCK) ON CK.ID = stockDetails.CarMakeId

		   
END
