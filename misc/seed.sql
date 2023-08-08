-- Thank you ChatGPT ^^

-- Insert pets
INSERT INTO public."Pets" ("Id", "Name", "PetType", "CreatedAt", "ModifiedAt", "CreatedBy", "ModifiedBy", "OwnerId") VALUES
  ('f47ac10b-58cc-4372-a567-0e02b2c3d479', 'Fluffy', 'Cat', NOW(), NOW(), 'system', 'system', 'graduenz@gmail.com'),
  ('54a9c7ec-546b-44d7-9616-e392ad1b1bd7', 'Buddy', 'Dog', NOW(), NOW(), 'system', 'system', 'paulo.monezi@icloud.com'),
  ('48b1f5c9-03c5-429f-bc72-9c6ac6f733a9', 'Whiskers', 'Cat', NOW(), NOW(), 'system', 'system', 'graduenz@gmail.com'),
  ('7c5c4ac5-af58-48c2-9a66-1c7ed5738282', 'Max', 'Dog', NOW(), NOW(), 'system', 'system', 'paulo.monezi@icloud.com'),
  ('e4d5d332-1cda-4a1a-98a9-75ef676fedea', 'Leo', 'Cat', NOW(), NOW(), 'system', 'system', 'graduenz@gmail.com'),
  ('2f3f5d87-5e21-4c68-aa96-77cc6d927148', 'Rocky', 'Dog', NOW(), NOW(), 'system', 'system', 'paulo.monezi@icloud.com'),
  ('a30f6d42-00a1-45b1-aa2d-7ce0d7b95eab', 'Luna', 'Cat', NOW(), NOW(), 'system', 'system', 'graduenz@gmail.com'),
  ('b417d50b-e1b6-4a9b-97f7-ccce9d70e849', 'Daisy', 'Dog', NOW(), NOW(), 'system', 'system', 'paulo.monezi@icloud.com'),
  ('fc50b2eb-7cd2-4c03-8c69-4a59fe94618c', 'Oliver', 'Cat', NOW(), NOW(), 'system', 'system', 'graduenz@gmail.com'),
  ('ab7ec7db-92d7-4bb4-90d1-7b8f1f4a7c8a', 'Charlie', 'Dog', NOW(), NOW(), 'system', 'system', 'paulo.monezi@icloud.com');

-- Insert vaccines
INSERT INTO public."Vaccines" ("Id", "Name", "Description", "PetType", "Duration", "CreatedAt", "ModifiedAt", "CreatedBy", "ModifiedBy") VALUES
  ('de305d54-75b4-431b-adb2-eb6b9e546014', 'Rabies', 'Protection against rabies virus', 'Cat', 365, NOW(), NOW(), 'system', 'system'),
  ('6ba7b810-9dad-11d1-80b4-00c04fd430c8', 'Parvo', 'Protection against parvovirus', 'Dog', 180, NOW(), NOW(), 'system', 'system'),
  ('eb591500-d0c3-4a4f-8c6d-539bc733e4e3', 'FVRCP', 'Combination vaccine for cats', 'Cat', 365, NOW(), NOW(), 'system', 'system'),
  ('8a4b221c-277d-457b-8800-96f77314cf66', 'DHPP', 'Combination vaccine for dogs', 'Dog', 180, NOW(), NOW(), 'system', 'system'),
  ('fabe9910-e7f7-44df-90e9-d7e0629b47d1', 'Lepto', 'Leptospirosis vaccine', 'Dog', 180, NOW(), NOW(), 'system', 'system'),
  ('a7ad51e2-68d2-45d8-94b8-bad8d09d8eaf', 'Bordetella', 'Protection against kennel cough', 'Dog', 180, NOW(), NOW(), 'system', 'system'),
  ('ab88e10d-b4e3-43b6-99c9-007ca74abbd2', 'FeLV', 'Feline leukemia vaccine', 'Cat', 365, NOW(), NOW(), 'system', 'system'),
  ('a81c4fb1-8564-421f-a5b5-d544b824e394', 'Lyme', 'Protection against Lyme disease', 'Dog', 180, NOW(), NOW(), 'system', 'system'),
  ('c46a6d2e-3fb9-43f9-874f-c2d7d318e90a', 'FIP', 'Feline infectious peritonitis vaccine', 'Cat', 365, NOW(), NOW(), 'system', 'system'),
  ('8e632d1e-9061-43a3-b5a8-71ce3b01c27c', 'Corona', 'Coronavirus vaccine', 'Dog', 180, NOW(), NOW(), 'system', 'system'),
  ('b8e733c3-9e1a-4c1f-8e57-37c722678e45', 'Distemper', 'Distemper vaccine', 'Dog', 365, NOW(), NOW(), 'system', 'system'),
  ('51abf4d5-1f60-4bcb-bb3d-029a0e60c28e', 'Calicivirus', 'Calicivirus vaccine', 'Cat', 365, NOW(), NOW(), 'system', 'system'),
  ('b5ce5360-1a75-4c5b-9878-dacc26db9f7d', 'Bordetella', 'Protection against kennel cough', 'Dog', 180, NOW(), NOW(), 'system', 'system'),
  ('d1e07dbb-90b4-4f2b-bf5c-7f109ac3a96b', 'FIP', 'Feline infectious peritonitis vaccine', 'Cat', 365, NOW(), NOW(), 'system', 'system'),
  ('65c968e3-8479-44af-aa32-72f81e9431ad', 'Corona', 'Coronavirus vaccine', 'Dog', 180, NOW(), NOW(), 'system', 'system'),
  ('7fb22683-6281-4253-950a-08c98848e05d', 'Rabies', 'Protection against rabies virus', 'Cat', 365, NOW(), NOW(), 'system', 'system'),
  ('a0ce3d3a-7993-4e00-8b08-24bf2d65abec', 'Parvo', 'Protection against parvovirus', 'Dog', 180, NOW(), NOW(), 'system', 'system'),
  ('49e00ad6-5f7d-4c57-9db3-09b945f68fde', 'FVRCP', 'Combination vaccine for cats', 'Cat', 365, NOW(), NOW(), 'system', 'system'),
  ('c69e6e84-6395-4c49-83ed-5d8d41d0a34f', 'DHPP', 'Combination vaccine for dogs', 'Dog', 180, NOW(), NOW(), 'system', 'system'),
  ('a22f7a12-7359-45a4-9243-5623c8bc8345', 'Lepto', 'Leptospirosis vaccine', 'Dog', 180, NOW(), NOW(), 'system', 'system');

-- Insert pet vaccinations
-- Distribute vaccinations among pets and owners
INSERT INTO public."PetVaccinations" ("Id", "PetId", "VaccineId", "AppliedAt", "CreatedAt", "ModifiedAt", "CreatedBy", "ModifiedBy") VALUES
  -- Pet 1
  ('bf16ea82-8e0f-4c6d-b8df-330f2b46807b', 'f47ac10b-58cc-4372-a567-0e02b2c3d479', 'de305d54-75b4-431b-adb2-eb6b9e546014', NOW() - INTERVAL '30 days', NOW(), NOW(), 'system', 'system'),
  ('65a5ebe0-6573-4bcf-b0c5-bce9bfb24453', 'f47ac10b-58cc-4372-a567-0e02b2c3d479', 'eb591500-d0c3-4a4f-8c6d-539bc733e4e3', NOW() - INTERVAL '90 days', NOW(), NOW(), 'system', 'system'),
  ('24a56e0b-7d20-4a5d-ba26-0d34403ce46d', 'f47ac10b-58cc-4372-a567-0e02b2c3d479', 'a7ad51e2-68d2-45d8-94b8-bad8d09d8eaf', NOW() - INTERVAL '150 days', NOW(), NOW(), 'system', 'system'),
  -- Pet 2
  ('05c4e4a1-26c3-48c7-bb59-cf63b66e1624', '54a9c7ec-546b-44d7-9616-e392ad1b1bd7', '6ba7b810-9dad-11d1-80b4-00c04fd430c8', NOW() - INTERVAL '60 days', NOW(), NOW(), 'system', 'system'),
  ('9c7ebbf2-8a8c-4359-9771-5d2d5f5261e2', '54a9c7ec-546b-44d7-9616-e392ad1b1bd7', 'fabe9910-e7f7-44df-90e9-d7e0629b47d1', NOW() - INTERVAL '100 days', NOW(), NOW(), 'system', 'system'),
  ('06c5e411-6a6f-4085-aebb-b3d7a201c97c', '54a9c7ec-546b-44d7-9616-e392ad1b1bd7', 'ab88e10d-b4e3-43b6-99c9-007ca74abbd2', NOW() - INTERVAL '180 days', NOW(), NOW(), 'system', 'system')
  -- ... Add more pet vaccinations here ...
  ;