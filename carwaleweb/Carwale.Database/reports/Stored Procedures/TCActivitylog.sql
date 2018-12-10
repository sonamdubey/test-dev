IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[TCActivitylog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[TCActivitylog]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 31/5/2012
-- Description:	Get Trading Car software dealers Activity log
-- =============================================
CREATE PROCEDURE Reports.TCActivitylog
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- To get login difference between succesive logins in Trading Car software
	DECLARE @DealerLog TABLE  
	(
	   DealerId bigint,
	   Logindiff int	   
	)
 
	 ;WITH base AS
     ( 
     SELECT  D.Id,UL.LoggedTime,
             ROW_NUMBER() over (partition BY D.Organization ORDER BY UL.LoggedTime) AS rn
     from TC_UsersLog as UL
     join TC_Users as U on U.BranchId=UL.BranchId and U.Id=UL.UserId
     join Dealers as D on D.Id=U.BranchId
     )
    
    -- To get login difference between last 2 logins in Trading Car software
    
    INSERT INTO @DealerLog(DealerId,Logindiff)
	SELECT   b1.Id,
			 isnull(AVG(DATEDIFF(DAY,b1.LoggedTime, b2.LoggedTime)),0)			 
	FROM     base b1
			 JOIN base b2
			 ON       b1.Id=b2.Id
			 AND      b2.rn        =b1.rn+1
	GROUP BY b1.Id
	ORDER BY b1.Id
	
	select Frequency, COUNT(*) as Cnt
	from (	
	select D.Organization,c.Name as City,   
	       case isnull(CPR.ConsumerId,0) when 0 then 'UnPaid' else 'Paid' end DealerType,
	       count(distinct TS.Id) as TCStockCount,
	       (
				SELECT count(distinct LL.Inquiryid) as CWStockCount
				FROM LiveListings as LL 
					join SellInquiries as SI on LL.Inquiryid=SI.ID and SI.DealerId=D.ID and SI.StatusId=1 and SI.PackageExpiryDate>GETDATE() 
			    WHERE SUBSTRING(LL.ProfileId,1,1)='D' 
			) as CWStockCount,
           MIN(UL.LoggedTime) as "First Login",
	       MAX(UL.LoggedTime) "Last Login" ,	       
	       DATEDIFF(Day,MAX(UL.LoggedTime),GETDATE()) "LastloginDur",
	       case 
	            when (DATEDIFF(Day,MAX(UL.LoggedTime),GETDATE())<=3) then 'Daily'	           
	            when  (DATEDIFF(Day,MAX(UL.LoggedTime),GETDATE()) between 4 and 10) then 'Weekly'
	            when (DATEDIFF(Day,MAX(UL.LoggedTime),GETDATE()) between 11 and 45) then 'Monthly'	            
	            else 'Random'	            
	         end as Frequency           
	from 
	   Dealers as D 
	   join TC_Stock as TS on TS.BranchId=D.ID and TS.StatusId=1 and TS.IsActive=1
	   left outer join TC_UsersLog as UL on D.Id=UL.BranchId	   
	   left outer join TC_Users as U on U.BranchId=UL.BranchId and U.Id=UL.UserId and UL.IpAddress not in (select IPAddress from Carwale.dbo.IPAddress)
	   left outer join ConsumerPackageRequests as CPR on CPR.ConsumerId=D.Id and CPR.isActive=1 and CPR.ConsumerType=1 and CPR.isApproved=1 and CPR.ActualAmount>0 
	   left outer join @DealerLog as DL on DL.DealerId=D.ID
       left outer join cities as C on C.Id=D.CityId
	 where D.Organization!='AEPL'	
	 and  UL.IpAddress not in ( select IpAddress from CarWale..IPAddress)
	 group by D.Organization,D.Id,CPR.ConsumerId,DL.Logindiff,C.Name
	)a
	group by Frequency

END
