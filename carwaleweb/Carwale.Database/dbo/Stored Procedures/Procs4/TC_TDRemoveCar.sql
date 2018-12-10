IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDRemoveCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDRemoveCar]
GO

	
-- Author:		Tejashree Patil
-- Create date: 22 June 2012
-- Description:	This will remove TD car from TD Car list for given branch
 --DECLARE @Status TINYINT
 --EXECUTE [TC_TDRemoveCar] '25','5',@Status OUTPUT
 --SELECT @Status
 -- Modified by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
 -- Modified by Ruchira Patil on 1st April 2016(Added status 27 and 28)
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDRemoveCar]
@TC_TDCarsId INT,
@BranchId BIGINT,
@Status TINYINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF EXISTS--checking here td_car id available on basis of records of TestDrive in TC_TDCalendar table
	(   SELECT  Top 1 C.TC_TDCarsId			
		FROM TC_TDCalendar C WITH(NOLOCK)
		INNER JOIN TC_CustomerDetails CD WITH(NOLOCK) ON C.TC_CustomerId=CD.Id
		WHERE C.BranchId=@BranchId AND C.TC_TDCarsId=@TC_TDCarsId AND C.TDStatus NOT IN (3,4,27,28)--Not Complete and canceled	
	)
	BEGIN
		SET @Status=2 --this TdCarId is not referenced with TC_TDCalendar table
	END
	ELSE 
	BEGIN
		SET @Status=1
		UPDATE TC_TDCars SET IsActive = 0 
		WHERE TC_TDCarsId=@TC_TDCarsId AND BranchId=@BranchId
		
	--below code is added by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
				
						    INSERT INTO TC_DealerMastersLog( DealerId,TableName,CreatedOn)
						    Values                         (@BranchId,'TC_TDCars',GETDATE())
	END
	
END
-------------------------------------------


