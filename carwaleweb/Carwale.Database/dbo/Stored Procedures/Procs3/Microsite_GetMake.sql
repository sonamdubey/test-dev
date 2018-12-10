IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetMake]
GO

	-- =============================================  
-- Author:  Umesh Ojha  
-- Create date: 3/5/2012  
-- Description: Fetching data for make of the available stock of the particular dealer in dealer website(microsite)  
-- =============================================  
CREATE PROCEDURE [dbo].[Microsite_GetMake]   
 -- Add the parameters for the stored procedure here  
 @DealerId BigInt  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 select distinct VM.Make,VM.MakeId 
 from vwMMV VM join   
 tc_stock TS on TS.VersionId=VM.VersionId 
 where TS.BranchId=@DealerId and TS.IsActive=1 
 and StatusId=1 order by VM.make  
END