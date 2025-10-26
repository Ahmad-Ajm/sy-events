-- بيانات اختبارية أساسية
INSERT INTO public.cities (id, name, name_en)
VALUES
  (gen_random_uuid(), 'دمشق', 'Damascus'),
  (gen_random_uuid(), 'حلب', 'Aleppo')
ON CONFLICT DO NOTHING;

INSERT INTO public.categories (id, name, name_en)
VALUES
  (gen_random_uuid(), 'مؤتمر', 'Conference'),
  (gen_random_uuid(), 'ورشة عمل', 'Workshop')
ON CONFLICT DO NOTHING;
