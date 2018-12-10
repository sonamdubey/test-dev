IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarValuationSuggestions_Bak]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedCarValuationSuggestions_Bak]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 17-01-2012
-- Description:	Populate ProfileIds matching with Used car Valuation customer search criteria
-- exec [dbo].[UsedCarValuationSuggestions] 270,'4/1/2009 12:00:00 AM',1,25000,221500
-- =============================================
CREATE PROCEDURE [dbo].[UsedCarValuationSuggestions_Bak]
@Version INT,@MakeYear DATETIME,@Cityid INT, @CarKm BIGINT,@CarPrice bigint
	-- Add the parameters for the stored procedure here
AS
BEGIN	
	SET NOCOUNT ON; 
	
	declare @Lattitude  bigint,  @Longitude  bigint	, @LattDiff bigint,@LongDiff bigint
	
	declare @Varmakeyear smallint
	set @Varmakeyear=YEAR(@makeyear)
	
	declare @CarKmDiff smallint
	set @CarKmDiff=5000
	
	declare @sql varchar(max), @Versions varchar(1000)
    declare @similarversion varchar(1000)
    
	declare @cnt int
	
	Create table #tempCars
	(
	   CarId tinyint identity(1,1),
	   ProfileId varchar(20),
	   Seller varchar(20),
	   CarName varchar(50),
	   AreaName varchar(50),
	   CityName varchar(20),
	   MakeYear datetime,
	   Price bigint,
	   Kilometers bigint,
	   FrontImagePath 	varchar(200),
	   HostURL varchar(50)
	)
	
	select @Lattitude = Lattitude,@Longitude=Longitude
	from Cities
	where Id=@Cityid
	
	set @LattDiff=100
	set @LongDiff=100	
	
	insert into #tempCars
	--select top 4 Seller,MakeName+' '+ModelName+' '+VersionName as CarName,CityName,MakeYear,Price,Kilometers
	select top 4 ProfileId,Seller,MakeName+' '+ModelName+' '+VersionName as CarName,CityName,AreaName,MakeYear,Price,Kilometers,FrontImagePath,HostURL
	from LiveListings	
	where  VersionId=@version AND year(MakeYear) between (@Varmakeyear-1)  and (@Varmakeyear+1)
	AND CityId=@cityid AND Kilometers  between @CarKm-5000 and @CarKm+5000
	AND Lattitude BETWEEN @Lattitude - @LattDiff AND @Lattitude + @LattDiff
	AND Longitude BETWEEN @Longitude - @LongDiff AND @Longitude + @LongDiff 
	AND Price between (@CarPrice*0.98) and (@CarPrice*1.2)
	Order by ABS(Price - @CarPrice)
		
	set @cnt=@@ROWCOUNT	
	set @cnt=4-@cnt
		
	if @cnt>0
	begin
	
		select @similarversion = SimilarVersions
		  from SimilarCars
		where VersionId=@version		
		
		IF (LEN(@similarversion)<1)  set @similarversion='0'
		
		--set @CarPrice=1000
		
		set @sql=' select top '+cast(@cnt as CHAR(2))+'  ProfileId,Seller,MakeName+''''+ModelName+''''+VersionName as CarName,CityName,AreaName,MakeYear,Price,Kilometers,FrontImagePath,HostURL '+
		--set @sql=' select top '+cast(@cnt as CHAR(2))+'  Seller,MakeName+''''+ModelName+''''+VersionName as CarName,CityName,MakeYear,Price,Kilometers'+
		' from LiveListings	as L '+	   
		' where  VersionId in ('+cast(@similarversion as varchar(max)) + ')'+
		' AND year(MakeYear) between '+Cast(@Varmakeyear as varchar(20))+'-1 AND '+cast(@Varmakeyear as varchar(20))+'+1 AND CityId='+cast(@cityid as varchar(10))+
		' AND Kilometers between '+cast(@CarKm as varchar(20))+ '-5000 AND '+cast(@CarKm as varchar(20))+'+ 5000 '+
		' AND Price between ('+ cast(@CarPrice as varchar(20))+'*0.98) and ('+cast(@CarPrice as varchar(20))+'*1.2)'+
		' AND Lattitude BETWEEN '+cast(@Lattitude as varchar(20))+' - '+cast(@LattDiff as varchar(20)) +'AND '+cast(@Lattitude as varchar(20)) +'+'+ cast(@LattDiff as varchar(20))+
		' AND Longitude BETWEEN '+cast(@Longitude as varchar(20))+' - '+cast(@LongDiff as varchar(20)) +'AND '+cast(@Longitude as varchar(20)) +'+'+ cast(@LongDiff as varchar(20))
		
		
		print @sql
		
		INSERT into #tempCars(ProfileId,Seller,CarName,CityName,AreaName,MakeYear,Price,Kilometers,FrontImagePath,HostURL) execute (@sql )
	
	end
	
	select @cnt=COUNT(*)
	from #tempCars
	set @cnt=4-@cnt
	
	if @cnt>0
	begin
		insert into #tempCars(ProfileId,Seller,CarName,CityName,AreaName,MakeYear,Price,Kilometers,FrontImagePath,HostURL)
		select top 4 ProfileId,Seller,MakeName+' '+ModelName+' '+VersionName as CarName,CityName,AreaName,MakeYear,Price,Kilometers,FrontImagePath,HostURL
		--select top 4 Seller,MakeName+' '+ModelName+' '+VersionName as CarName,CityName,MakeYear,Price,Kilometers
		from LiveListings	
		where CityId=@cityid
		AND Lattitude BETWEEN @Lattitude - @LattDiff AND @Lattitude + @LattDiff
	    AND Longitude BETWEEN @Longitude - @LongDiff AND @Longitude + @LongDiff  
		Order by ABS(Price - @CarPrice)
	
	end
	
	
	select top 4 * from #tempCars
	
	drop table #tempCars
	
   
END


