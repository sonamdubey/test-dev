IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDateDescr]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetDateDescr]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 12/7/2012
-- Description:	Return date Description
-- =============================================
CREATE FUNCTION [dbo].[GetDateDescr]
(
	@Date date
)
RETURNS Varchar(50)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @curDate date, @Result Varchar(10),@diff int

	----Today
SELECT @curDate=GETDATE() 
SELECT @diff=datediff(dd,@Date,getdate())
If @curDate=@Date
set @Result='Today'
else if datediff(dd,@Date,getdate())=1
set @Result='Yesterday'
else if datediff(wk,@Date,getdate()) =0
set @Result='This Week'
else if datediff(wk,@Date,getdate()) =1
set @Result='Last Week'
else if datediff(mm,@Date,getdate()) =0
set @Result='This Month'
else if datediff(mm,@Date,getdate()) =1
set @Result='Last Month'
else if datediff(yy,@Date,getdate()) =0
set @Result='This Year'
else if datediff(yy,@Date,getdate()) =1
set @Result='Last Year'

--------First Day of Current Week
--else if DATEADD(wk,DATEDIFF(wk,0,GETDATE()),0)=@Date
--set @Result= 'First Day of Current Week'
--------Last Day of Current Week
--else if DATEADD(wk,DATEDIFF(wk,0,GETDATE()),6)=@Date 
--set @Result='Last Day of Current Week'
--------First Day of Last Week 
--else if DATEADD(wk,DATEDIFF(wk,7,GETDATE()),0)=@Date 
--set @Result= 'First Day of Last Week'
--------Last Day of Last Week 
--else if DATEADD(wk,DATEDIFF(wk,7,GETDATE()),6)=@Date 
--set @Result= 'Last Day of Last Week'
--------First Day of Current Month
--else if DATEADD(mm,DATEDIFF(mm,0,GETDATE()),0)=@Date 
--set @Result= 'First Day of Current Month'
--------Last Day of Current Month
--else if DATEADD(ms,- 3,DATEADD(mm,0,DATEADD(mm,DATEDIFF(mm,0,GETDATE())+1,0)))=@Date 
--set @Result= 'Last Day of Current Month'
--------First Day of Last Month
--else if DATEADD(mm,-1,DATEADD(mm,DATEDIFF(mm,0,GETDATE()),0))=@Date 
--set @Result= 'First Day of Last Month'
--------Last Day of Last Month 
--else if DATEADD(ms,-3,DATEADD(mm,0,DATEADD(mm,DATEDIFF(mm,0,GETDATE()),0))) =@Date 
--set @Result='Last Day of Last Month'
--------First Day of Current Year
--else if DATEADD(yy,DATEDIFF(yy,0,GETDATE()),0) =@date 
--set @Result='First Day of Current Year'
--------Last Day of Current Year
--else if DATEADD(ms,-3,DATEADD(yy,0,DATEADD(yy,DATEDIFF(yy,0,GETDATE())+1,0)))=@Date 
--set @Result= 'Last Day of Current Year'
--------First Day of Last Year
--else if DATEADD(yy,-1,DATEADD(yy,DATEDIFF(yy,0,GETDATE()),0)) =@Date 
--set @Result='First Day of Last Year'
--------Last Day of Last Year 
--else if DATEADD(ms,-3,DATEADD(yy,0,DATEADD(yy,DATEDIFF(yy,0,GETDATE()),0)))=@Date 
--set @Result= 'Last Day of Last Year'

	-- Return the result of the function
	RETURN @Result

END
