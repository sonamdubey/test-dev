IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TempInquiriesBulkUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TempInquiriesBulkUpdate]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <20/09/2012>
-- Description:	<First time update of calculated EMI in the LiveListings table>
-- =============================================
CREATE	 PROCEDURE [dbo].[TempInquiriesBulkUpdate]
as

-- Declare table variable
declare  @Inquiry table(id int identity(1,1),SellInquiryId Bigint)

declare @SellInquiryId int

	insert into @Inquiry(SellInquiryId)
	SELECT SI.ID
	FROM SellInquiries SI
	INNER JOIN TC_Stock TS ON TS.Id=SI.TC_StockId
	WHERE SI.CalculatedEMI IS NOT NULL AND TS.IsSychronizedCW=1  
	--Select query
	
declare @rowCount smallint,@loopCount smallint=1
select @rowCount=COUNT(*) from @Inquiry



while @rowCount>=@loopCount 

begin
	
	select @SellInquiryId=SellInquiryId from @Inquiry	WHERE ID=@loopCount	
		
	UPDATE LiveListings
	SET CalculatedEMI=SI.CalculatedEMI
	FROM SellInquiries SI
	     INNER JOIN  LiveListings LL ON LL.Inquiryid=SI.ID  AND LL.SellerType=1
	WHERE LL.InquiryId=@SellInquiryId
	
	set @loopCount=@loopCount+1
end



