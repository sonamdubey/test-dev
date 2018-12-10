IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_GetPriceQuoteDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_GetPriceQuoteDetails]
GO

	
CREATE procedure [dbo].[NCD_GetPriceQuoteDetails]  
(  
@enq_id int  
)  
as  
--New Procedure created by : Surendra
--Purpose : Getting Price Quote Details for particular City and Version

declare @city_id int,@version_id int  
  
-- getting verion_id and city id for particular enq  
select @city_id=CityId,@version_id=VersionId from NCD_Inquiries  
where Id=@enq_id  
  
 --  fetching price details  
 if ((@city_id is null) or (@version_id is null))  
 begin  
	return -1  
 end  
 else  
 begin 
	 
	 -- For Getting car Name
	 declare @car varchar(100)
	 
	 select @car =(M.Name + ' ' + CM.Name + ' ' + CV.Name)
	 from CarVersions CV inner join CarModels CM on CV.CarModelId=CM.ID
	 inner join CarMakes M on CM.CarMakeId=M.ID
	 where CV.ID=@version_id
	 
	 select Price,RTO,Insurance,(Price+RTO+Insurance) 'TotalPrice',@car 'Car',
	 (select Name from Cities where ID=@city_id) 'City'   from NewCarShowroomPrices  
	 where CarVersionId=@version_id and CityId=@city_id
	 
	 return 0  
 end
