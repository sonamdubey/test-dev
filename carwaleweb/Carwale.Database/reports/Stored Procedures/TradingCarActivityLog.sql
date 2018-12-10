IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[TradingCarActivityLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[TradingCarActivityLog]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 03-04-2012
-- Description:	Trading Car Activity
-- =============================================
CREATE PROCEDURE Reports.TradingCarActivityLog
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	declare @DealerLog Table  
	(
	   DealerId bigint,
	   Logindiff int	   
	)
    -- Insert statements for procedure here
	 ;WITH base AS
     ( 
     SELECT  D.Id,UL.LoggedTime,
             ROW_NUMBER() over (partition BY D.Organization ORDER BY UL.LoggedTime) AS rn
     from TC_UsersLog as UL
     join TC_Users as U on U.BranchId=UL.BranchId and U.Id=UL.UserId
     join Dealers as D on D.Id=U.BranchId
     )
    
    INSERT INTO @DealerLog(DealerId,Logindiff)
	SELECT   b1.Id,
			 AVG(DATEDIFF(DAY,b1.LoggedTime, b2.LoggedTime) ) AS Logindiff			 
	FROM     base b1
			 JOIN base b2
			 ON       b1.Id=b2.Id
			 AND      b2.rn        =b1.rn+1
	GROUP BY b1.Id
	ORDER BY b1.Id
	
	select D.Organization,IP.Name as PackageName,count(TS.Id) as StockCount,
	       MAX(UL.LoggedTime) "Last Login" ,DATEDIFF(Day,MAX(UL.LoggedTime),GETDATE()) " Time since last login ( in days)",
	       case DL.Logindiff
	            when 0 then 'Daily'
	            when 1 then 'Daily'
	            when 2 then 'Daily'
	            when 3 then 'Weekly'
	            when 4 then 'Weekly'
	            when 5 then 'Weekly'
	            when 6 then 'Weekly'
	            when 7 then 'Weekly'
	           else 'Monthly'
	         end as Frequency            
	from TC_UsersLog as UL
	   join TC_Users as U on U.BranchId=UL.BranchId and U.Id=UL.UserId
	   join Dealers as D on D.Id=U.BranchId
	   join TC_Stock as TS on TS.BranchId=D.ID and TS.StatusId=1 and TS.IsActive=0
	   join ConsumerCreditPoints as CP on CP.ConsumerId=UL.BranchId and CP.ConsumerType=1
	   join InquiryPointCategory as IP on IP.Id=CP.PackageType
	   join @DealerLog as DL on DL.DealerId=D.ID
	 where D.Organization!='AEPL'	
	 group by D.Organization,D.Id,IP.Name,DL.Logindiff
	 order by DATEDIFF(Day,MAX(UL.LoggedTime),GETDATE()) desc
 
END
