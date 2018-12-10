IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[GetDealerInvoiceAddress]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[GetDealerInvoiceAddress]
GO

	CREATE PROCEDURE [reports].[GetDealerInvoiceAddress]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @date datetime
    set @date = convert(datetime,convert(varchar(10),GETDATE(),120)+ ' 00:00:00')	



    
 --   select D.ContactPerson,D.Organization,D.Address1,D.Address2,A.Name as Area,C.Name as City,S.Name as [state],D.Pincode,ltrim(rtrim(D.MobileNo)) as MobileNo,D.PhoneNo 
	--from dbo.Dealers as D
	--	 join Areas as A on A.ID=D.AreaId
	--	 Join Cities as C on D.CityId=C.Id
	--	 join States as S on S.ID=D.StateId
	--	 join ActiveDealers as AD on AD.DealerId=D.ID
	--where  AD.isActive=1
	
	select D.ContactPerson,D.Organization,D.Address1,D.Address2,A.Name as Area,C.Name as City,S.Name as [state],D.Pincode,ltrim(rtrim(D.MobileNo)) as MobileNo,D.PhoneNo 
     	from dbo.Dealers as D
		 join Areas as A on A.ID=D.AreaId
		 Join Cities as C on D.CityId=C.Id
		 join States as S on S.ID=D.StateId		 
	where  D.ID in (
	SELECT D.ID AS ID
	FROM Dealers AS D 
	JOIN SellInquiries Si ON D.Id = Si.DealerId AND StatusId = 1 AND PackageExpiryDate >= @date
	JOIN Areas AS A on A.ID = D.AreaId
	JOIN Cities AS C on C.ID = D.CityId
	WHERE D.Status=0 
	AND D.ID NOT IN(SELECT DealerId FROM DeletedDealers) 
	GROUP BY D.ID, D.Organization, D.LogoUrl, JoiningDate, A.Name, C.Name, D.PhoneNo, D.MobileNo, D.EmailId,D.IsTCDealer  
	having COUNT(Si.Id)>0
	)
	order by D.Organization


END
