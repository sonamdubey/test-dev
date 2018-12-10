IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ncdsp_enquiries_insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ncdsp_enquiries_insert]
GO

	
CREATE procedure [dbo].[ncdsp_enquiries_insert]    
(    
@dealer_id int, 
@city_id int = null,    
@version_id int,   
@by_plan varchar(30) =null,    
@cname varchar(50),    
@email varchar(100),    
@mobile varchar(10),    
@req_type tinyint ,  
@query text = null
)    
as    
begin    
insert into dbo.NCD_Enquiries   
(enq_dealer_id,enq_city_id,enq_version_id,enq_buy_plan,enq_cname,enq_email,enq_mobile,enq_req_type,enq_query,enq_req_datetime)    
values(@dealer_id,@city_id,@version_id,@by_plan,@cname,@email,@mobile,@req_type,@query,GETDATE())   
return @@identity  
end 

