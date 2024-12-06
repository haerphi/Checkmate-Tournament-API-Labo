﻿-- AddPlayer errors
EXEC sp_addmessage 
    @msgnum = 50001, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Nickname already used', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50002, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Email already used', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50003, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Elo must be between 0 and 3000', -- Error message
    @lang = 'us_english'; -- Language
GO;

-- CreateTournament errors
EXEC sp_addmessage 
    @msgnum = 50004, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Minimum number of players cannot be greater than Maximum number of players', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50005, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Minimum ELO cannot be greater than Maximum ELO', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50006, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'End inscription date must be in the future', -- Error message
    @lang = 'us_english'; -- Language
GO;

-- ChangePlayerPassword errors
EXEC sp_addmessage 
    @msgnum = 50007, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Player does not exist', -- Error message
    @lang = 'us_english'; -- Language