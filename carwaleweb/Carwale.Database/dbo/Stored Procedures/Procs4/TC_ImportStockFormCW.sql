IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ImportStockFormCW]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ImportStockFormCW]
GO

	-- =============================================
-- Modified By: Binumon George
-- Modified date: 17th Apr,2012
-- Description:	checking dealertypeid before inserting Tc_roletask table

-- =============================================
-- Modified By: Binumon George
-- Modified date: 4th Apr,2012
-- Modified By: Surendar on 13 march for changing roles table
-- Description:	insert combination of roleid and taskid in TC_RoleTasks table
-- =============================================
CREATE  PROCEDURE       [dbo].[TC_ImportStockFormCW]
@Dealer_ID Numeric,
@StockIdChain varchar(max),
@Status Numeric Output, --For checking already data available or not in Tc_stock table
@Datatransfered Numeric Output-- For number of records affected
As
/*
Begin

print 1
	SET NOCOUNT ON  
	Declare @rowcount smallint	
	Declare @BranchID int
	Declare @ROLEID		NUMERIC
	
	SET @Status = 0 -- Default status will be zero
	
	BEGIN TRY
		BEGIN TRANSACTION TranStock
	
	--Condition:1: If dealer is a trading cars application user.
	--Condition:2: Import existing stock from CarWale only if Trading Cars stock is empty or no car in stock
	 IF EXISTS(SELECT Id FROM Dealers D WHERE ID = @Dealer_ID AND IsTCDealer = 1)
		BEGIN
			
			-- Check if dealer is registered with trading cars
			-- If not registered one available in 'Dealers' table
			IF NOT EXISTS(SELECT TOP 1 * FROM TC_Users WHERE BranchId=@Dealer_ID AND IsActive=1)
			BEGIN			
				-- Following code block is used for When Dealer have multiple outlet
				DECLARE @isMultiOutlet bit
				SELECT @isMultiOutlet=IsMultiOutlet FROM Dealers WHERE ID=@Dealer_ID
					
				/*IF (@isMultiOutlet=1) -- Meaning Dealer has multiple outlet
				BEGIN
					INSERT INTO TC_DealerAdmin(Organization) SELECT Organization FROM Dealers WHERE ID=@Dealer_ID
					INSERT INTO TC_DealerAdminMapping(DealerAdminId,DealerId) VALUES(@@IDENTITY,@Dealer_ID)
				END*/
				
				--Here we are creating new roll with name SUPER Admin which have permission to to all task
				--DECLARE @listTask VARCHAR(MAX) -- This variable is used to fetch comma seperated task id
				--SELECT @listTask = COALESCE(@listTask+',' ,'') + convert(VARCHAR,Id) FROM TC_Tasks -- assiging comma seperated task id in variable @listStr
				
				-- Creating new roll for current dealer
				INSERT INTO TC_Roles(BranchId,RoleName,RoleDescription/*,TaskSet*/,RoleCreationDate)
				VALUES(@Dealer_ID,'Super Admin','Super Admin'/*,@listTask*/,GETDATE())
				SET @ROLEID=SCOPE_IDENTITY()
				DECLARE @DealerType TINYINT
				SELECT @DealerType=TC_DealerTypeId FROM Dealers WHERE ID=@Dealer_ID-- taking dealertype from dealer table
				--Inserting taskset to TC_RoleTasks	
				/*				
				INSERT INTO TC_RoleTasks(RoleId,TaskId) SELECT @ROLEID, T.ID FROM TC_Tasks T
				WHERE T.TC_DealerTypeId=@DealerType OR T.TC_DealerTypeId IS NULL
				this code was befoe used upto the date of 17-04-2012
				*/
				-- this code done by binu 
				IF(@DealerType=0 OR @DealerType IS NULL)-- for normal ucd dealer
					BEGIN
						SET @DealerType=1
						INSERT INTO TC_RoleTasks(RoleId,TaskId) SELECT @ROLEID, T.ID FROM TC_Tasks T
						WHERE T.Id<>3 AND (T.TC_DealerTypeId=@DealerType OR T.TC_DealerTypeId IS NULL)
					END
				ELSE IF(@DealerType =3)-- for  ucd and ncd dealer
					BEGIN
						INSERT INTO TC_RoleTasks(RoleId,TaskId) SELECT @ROLEID, T.ID FROM TC_Tasks T
					END
				ELSE IF(@DealerType !=0)-- for  ucd or ncd dealer
					BEGIN
						INSERT INTO TC_RoleTasks(RoleId,TaskId) SELECT @ROLEID, T.ID FROM TC_Tasks T
						WHERE T.TC_DealerTypeId=@DealerType OR T.TC_DealerTypeId IS NULL
					END
				-- Inserting record in TC_users table from Dealers table
				INSERT INTO TC_Users(BranchId,RoleId,UserName,Email,Password,Mobile,EntryDate,DOJ,Address)
				SELECT ID,@ROLEID/*Super Admin*/,RTRIM(FirstName + ' ' + LastName),EmailId,Passwd,MobileNo,GETDATE(),JoiningDate,Address1 FROM Dealers WHERE ID=@Dealer_ID
			END
			
			
			DECLARE @Separator VARCHAR(1) = ','
			DECLARE @Separator_position INT -- This is used to locate each separator character  
			DECLARE @SellInquiryId VARCHAR(15) -- this holds each array value as it is returned
			DECLARE @StockId INT			
			
			SET @StockIdChain = @StockIdChain + @Separator 
			SET @Datatransfered=0
			WHILE PATINDEX('%' + @Separator + '%', @StockIdChain) <> 0   
			BEGIN 
				SELECT  @Separator_position = PATINDEX('%' + @Separator + '%',@StockIdChain)  
				SELECT  @SellInquiryId = LEFT(@StockIdChain, @Separator_position - 1)
				
				SELECT @StockId=TC_StockId FROM SellInquiries WHERE ID=@SellInquiryId
				IF(@StockId IS NULL) --IF stock id not available in sellinquiry table insert new stock
					BEGIN
						EXECUTE TC_ImportStockFormCW_Insert @SellInquiryId, @Dealer_ID
					END
				ELSE
					BEGIN --IF stock id  available in sellinquiry table update stock
						EXECUTE TC_ImportStockFormCW_Update @SellInquiryId, @StockId
					END		
				SET @Datatransfered=@Datatransfered+1
				SELECT  @StockIdChain = STUFF(@StockIdChain, 1, @Separator_position, '') 
			END
			
			UPDATE Dealers Set Passwd = Passwd + Convert(VARCHAR,DATEPART(Year,GETDATE())) WHERE ID = @BranchID
			SET @Status=1 -- Stock Import Done
		End
	Else
		Begin
			-- Stock import operation faild because of following reason
			-- Dealer is not marked as Trading Car Dealer in Dealers table			
			set @status=2				
		End		
		SET NOCOUNT OFF
		
		COMMIT TRANSACTION TranStock
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRANSACTION TranStock
		--SELECT ERROR_NUMBER() AS ErrorNumber;
	END CATCH;
	
End --Root BeginEnd
*/
