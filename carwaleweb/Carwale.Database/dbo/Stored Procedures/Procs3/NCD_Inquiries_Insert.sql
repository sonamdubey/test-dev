IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_Inquiries_Insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_Inquiries_Insert]
GO

	
CREATE procedure [dbo].[NCD_Inquiries_Insert]    
(    
@DealerId int, 
@CustomerId int,
@Cityid int = null,    
@VersionId int,   
@BuyPlan varchar(30) =null,   
@ReqType tinyint ,  
@Query text = null,
@InquirySource tinyInt
)    
as    
begin    
--New Procedure created by : Surendra
--Purpose : Inserting all enquiry comming from the customer to the NCD

insert into dbo.NCD_Inquiries   
(DealerId,CustomerId,CityId,VersionId,BuyPlan,RequestType,InquiryDescription,EntryDate,InquirySource)    
values(@DealerId,@CustomerId,@Cityid,@VersionId,@BuyPlan,@ReqType,@Query,GETDATE(),@InquirySource)   
return @@identity
end 



