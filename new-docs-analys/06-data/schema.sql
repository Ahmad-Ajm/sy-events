-- PostgreSQL schema (Event Management)

CREATE TABLE IF NOT EXISTS public.cities (
  id UUID PRIMARY KEY,
  name TEXT NOT NULL,
  name_en TEXT NOT NULL,
  created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
  updated_at TIMESTAMPTZ
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_cities_name ON public.cities(name);
CREATE UNIQUE INDEX IF NOT EXISTS ux_cities_name_en ON public.cities(name_en);

CREATE TABLE IF NOT EXISTS public.categories (
  id UUID PRIMARY KEY,
  name TEXT NOT NULL,
  name_en TEXT NOT NULL,
  description TEXT,
  description_en TEXT,
  created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
  updated_at TIMESTAMPTZ
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_categories_name ON public.categories(name);
CREATE UNIQUE INDEX IF NOT EXISTS ux_categories_name_en ON public.categories(name_en);

CREATE TABLE IF NOT EXISTS public.users (
  id UUID PRIMARY KEY,
  email TEXT NOT NULL,
  name TEXT NOT NULL,
  password_hash TEXT NOT NULL,
  phone TEXT,
  profession TEXT,
  city_id UUID NULL REFERENCES public.cities(id) ON DELETE SET NULL,
  interests TEXT,
  reason TEXT,
  role INT NOT NULL,
  created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
  updated_at TIMESTAMPTZ
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_users_email ON public.users(email);

CREATE TABLE IF NOT EXISTS public.events (
  id UUID PRIMARY KEY,
  title TEXT NOT NULL,
  title_en TEXT,
  description TEXT NOT NULL,
  description_en TEXT,
  start_date TIMESTAMPTZ NOT NULL,
  end_date TIMESTAMPTZ NOT NULL,
  location TEXT NOT NULL,
  location_en TEXT,
  max_capacity INT,
  is_approved BOOLEAN NOT NULL DEFAULT FALSE,
  status INT NOT NULL,
  image_url TEXT,
  thumbnail_url TEXT,
  category_id UUID NOT NULL REFERENCES public.categories(id) ON DELETE RESTRICT,
  city_id UUID NOT NULL REFERENCES public.cities(id) ON DELETE RESTRICT,
  organizer_id UUID NOT NULL REFERENCES public.users(id) ON DELETE RESTRICT,
  created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
  updated_at TIMESTAMPTZ
);
CREATE INDEX IF NOT EXISTS ix_events_start_date ON public.events(start_date);
CREATE INDEX IF NOT EXISTS ix_events_city_category ON public.events(city_id, category_id);

CREATE TABLE IF NOT EXISTS public.bookings (
  id UUID PRIMARY KEY,
  user_id UUID NOT NULL REFERENCES public.users(id) ON DELETE CASCADE,
  event_id UUID NOT NULL REFERENCES public.events(id) ON DELETE CASCADE,
  status INT NOT NULL,
  reminder_time INT,
  attended_at TIMESTAMPTZ,
  created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
  updated_at TIMESTAMPTZ
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_bookings_user_event ON public.bookings(user_id, event_id);
CREATE INDEX IF NOT EXISTS ix_bookings_event_id ON public.bookings(event_id);

CREATE TABLE IF NOT EXISTS public.event_files (
  id UUID PRIMARY KEY,
  event_id UUID NOT NULL REFERENCES public.events(id) ON DELETE CASCADE,
  file_name TEXT NOT NULL,
  file_path TEXT NOT NULL,
  file_type TEXT,
  mime_type TEXT,
  alt TEXT,
  thumbnail_path TEXT,
  display_order INT,
  created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);
