IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateUsedCarMasterColorJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateUsedCarMasterColorJob]
GO

	---Created By: Manish Chourasiya on 23-09-2014 
---Desc : It will update the Used car color id for the new records and will execute at night daily.
CREATE PROCEDURE [dbo].[UpdateUsedCarMasterColorJob]
AS
   BEGIN

	/*	UPDATE SellInquiries SET UsedCarMasterColorsId=dbo.GetMasterUsedCarColorFromUserColor(Color)
		WHERE UsedCarMasterColorsId IS NULL;

		UPDATE CustomerSellInquiries SET UsedCarMasterColorsId=dbo.GetMasterUsedCarColorFromUserColor(Color)
		WHERE UsedCarMasterColorsId IS NULL;
    */

		UPDATE livelistings SET UsedCarMasterColorsId=dbo.GetMasterUsedCarColorFromUserColor(Color)
		WHERE UsedCarMasterColorsId IS NULL;


    END 
