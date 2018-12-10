IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDescriptionofUsedCarwithOffers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDescriptionofUsedCarwithOffers]
GO

	-- =============================================      
-- Author:  Raghu      
-- Create date: 16-1-2014      
-- Description: Gets the description of used car which has got offers     
-- =============================================      
CREATE PROCEDURE [dbo].[GetDescriptionofUsedCarwithOffers]  --EXEC [GetDescriptionofUsedCarwithOffers]   8912 
(          
	@InquiryId NUMERIC      
)      
 AS  
BEGIN      
	SELECT UCD.Description,UCD.OfferName, convert(varchar(12),UCD.EndDate,106) AS EndDate
	FROM  TC_UsedCarOffers UCD WITH(NOLOCK)
	INNER JOIN TC_MappingOfferWithStock MOS WITH(NOLOCK) ON UCD.TC_UsedCarOfferId = MOS.TC_UsedCarOfferId AND MOS.IsActive = 1
	INNER JOIN SellInquiries SI WITH(NOLOCK) ON  SI.TC_StockId = MOS.StockId 
	WHERE UCD.IsActive =1 AND SI.ID=@InquiryId
	ORDER BY MOS.EndDate desc
END

     
      
      
