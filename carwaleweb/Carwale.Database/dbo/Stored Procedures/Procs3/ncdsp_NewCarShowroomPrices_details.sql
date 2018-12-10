IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ncdsp_NewCarShowroomPrices_details]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ncdsp_NewCarShowroomPrices_details]
GO

	
CREATE procedure [dbo].[ncdsp_NewCarShowroomPrices_details]  
(  
@enq_id int  
)  
as  
declare @city_id int,@version_id int  
  
-- getting verion_id and city id for particular enq  
select @city_id=enq_city_id,@version_id=enq_version_id from NCD_Enquiries  
where enq_id=@enq_id  
  
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
