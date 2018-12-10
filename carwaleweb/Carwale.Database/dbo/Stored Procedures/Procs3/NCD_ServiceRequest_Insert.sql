IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_ServiceRequest_Insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_ServiceRequest_Insert]
GO

	
CREATE procedure [dbo].[NCD_ServiceRequest_Insert]    
(    
@DealerId int, 
@CustomerId int,    
@VersionId int,   
@RegNo varchar(50) ,
@PreferredDate datetime,
@ServiceType smallint,
@Comments varchar(250)
)    
as    
begin    
--New Procedure created by : Umesh
--Purpose : Inserting data comming from the service request

insert into dbo.NCD_ServiceRequest   
(DealerId,CustomerId,VersionId,RegNo,PreferredDate,RequestDate,TypeOfService,Comments)    
values(@DealerId,@CustomerId,@VersionId,@RegNo,@PreferredDate,GETDATE(),@ServiceType,@Comments)   
return @@identity
end 



