IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PriceQuote_GetModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PriceQuote_GetModel]
GO

	-- Create date: <9/2/2012>  
-- Description: <Description,,>  
-- AM Modified  06-09-2012 not to show certain models to dealers 
--Modified By <Rakesh Yadav,10 Sep 2014>
--Carversion is joined with NewCarShowroomPrices instead of Con_NewCarNationalPrices AND Mo.new=1 is filter added 
-- EXEC [dbo].[PriceQuote_GetModel]  5,10
-- =============================================  

CREATE PROCEDURE [dbo].[PriceQuote_GetModel]   
 -- Add the parameters for the stored procedure here  
 (  
 @DealerId INT,  
 @MakeId int   
 )  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
    -- Insert statements for procedure here  
 SELECT DISTINCT VM.Model Text, VM.ModelId Value   
FROM vwMMV        VM 
JOIN NewCarShowroomPrices NCSP  WITH(NOLOCK) ON VM.VersionId=NCSP.CarVersionId
WHERE 
VM.MakeId=@MakeId
AND NCSP.IsActive = 1 AND VM.IsModelNew=1 AND VM.IsVerionNew=1
-- AM Modified  06-09-2012 not to show certain models to dealers 
AND VM.ModelId NOT IN ( SELECT ModelId FROM TC_NoDealerModels WITH(NOLOCK) WHERE DealerId =@DealerId )   
ORDER BY Text  
END  