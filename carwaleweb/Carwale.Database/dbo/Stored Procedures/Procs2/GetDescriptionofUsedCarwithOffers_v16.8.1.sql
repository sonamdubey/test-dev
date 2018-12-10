IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDescriptionofUsedCarwithOffers_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDescriptionofUsedCarwithOffers_v16]
GO

	-- =============================================      
-- Author:  Raghu      
-- Create date: 16-1-2014      
-- Description: Gets the description of used car which has got offers 
-- Modified by Prachi Phalak on 04/08/2016 --Added check of sourceId for Autobiz cars.    
-- =============================================      
CREATE PROCEDURE [dbo].[GetDescriptionofUsedCarwithOffers_v16.8.1]  --EXEC [GetDescriptionofUsedCarwithOffers]   8912 
(          
	@InquiryId INT      
)      
 AS  
BEGIN      
	SELECT UCD.Description,UCD.OfferName, convert(varchar(12),UCD.EndDate,106) AS EndDate
	FROM  TC_UsedCarOffers UCD WITH(NOLOCK)
	INNER JOIN TC_MappingOfferWithStock MOS WITH(NOLOCK) ON UCD.TC_UsedCarOfferId = MOS.TC_UsedCarOfferId AND MOS.IsActive = 1
	INNER JOIN SellInquiries SI WITH(NOLOCK) ON  SI.TC_StockId = MOS.StockId AND SI.SourceId = 2
	WHERE UCD.IsActive =1 AND SI.ID=@InquiryId
	ORDER BY MOS.EndDate desc
END

     
      
      

